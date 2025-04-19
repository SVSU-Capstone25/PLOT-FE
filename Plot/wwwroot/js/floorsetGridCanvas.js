import Fixture from "./Fixture.js";
import EmployeeArea from "./EmployeeArea.js";
import Grid from "./Grid.js";
import { getFixtureInstances, getEmployeeAreas } from "./getters.js";
import {
  updateFixtureInstance,
  createFixtureInstance,
  createEmployeeAreas,
  deleteEmployeeAreas,
} from "./setters.js";

/**
 * @typedef {import('./globals.d.ts')}
 */

/** @type {Grid | null} */
let gridInstance;
let p5Instance;

/**
 * Parses the cookies from the browser
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Record<string, string>} Cookies in a indexable object
 */
function getCookies() {
  const cookies = document.cookie.split(";");

  return Object.fromEntries(
    cookies.map((cookie) => {
      const [name, value] = cookie.split("=").map((part) => part.trim());
      return [name, decodeURIComponent(value)];
    })
  );
}

/**
 * Gets the cookie from the browser
 * @argument {string} key Name of cookie
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<string>}
 */
async function getCookie(key) {
  const cookies = getCookies();

  if (!cookies[key]) throw new Error(`The ${key} cookie isn't available!`);

  return cookies[key];
}

function createDebouncedAggregator(wait, onFlush) {
  const items = new Set();
  let timerId = null;

  return function add(item) {
    items.add(JSON.stringify(item));
    clearTimeout(timerId);
    timerId = setTimeout(() => {
      // snapshot the set so your callback can mutate it if needed
      const snapshot = Array.from(items).map(JSON.parse);
      onFlush(snapshot);
      items.clear();
    }, wait);
  };
}

function onZoomScroll(event) {
  if (event.deltaY > 0) {
    gridInstance.scale += 0.1;
  } else {
    gridInstance.scale -= 0.1;
  }

  gridInstance.scale = Math.max(0.1, gridInstance.scale);
  gridInstance.resize();
}

function sketch(p5) {
  let mouseFixture, floorsetId;

  const paintAggregator = createDebouncedAggregator(500, (fixtures) => {
    fixtures.forEach((fixture) => {
      updateFixtureInstance(fixture).then(console.log).catch(console.error);
    });
  });

  const addEmployeeAreaPaintAggregator = createDebouncedAggregator(
    500,
    (employeeAreas) => {
      createEmployeeAreas(employeeAreas)
        .then(() => gridInstance.addEmployeeAreas(employeeAreas))
        .catch(console.error);
    }
  );

  const deleteEmployeeAreaPaintAggregator = createDebouncedAggregator(
    500,
    (employeeAreas) => {
      deleteEmployeeAreas(employeeAreas)
        .then(() => gridInstance.deleteEmployeeAreas(employeeAreas))
        .catch(console.error);
    }
  );

  p5.preload = () => {
    p5Instance = p5;
    gridInstance = new Grid(p5);
    window.grid = {};

    window.p5Instance = p5Instance;
    window.gridInstance = gridInstance;

    window.grid.state = "place";
    window.grid.paint = {
      COLOR: "#fff",
      SUPERCATEGORY_TUID: 0,
    };

    const url = new URL(window.location.href);

    floorsetId = url.pathname
      .replace("/floorset-editor/", "")
      .split("/")
      .map(Number)[1];

    getEmployeeAreas(floorsetId)
      .then((employeeAreas) => {
        employeeAreas.forEach((employeeArea) => {
          gridInstance.employeeAreas.set(
            [employeeArea.X_POS, employeeArea.Y_POS].join("-"),
            EmployeeArea.from(p5, employeeArea)
          );
        });
      })
      .catch(console.error);

    getFixtureInstances(floorsetId)
      .then((fixtureInstances) => {
        console.log(fixtureInstances);
        fixtureInstances.forEach((fixtureInstance) => {
          gridInstance.fixtures.push(Fixture.from(p5, fixtureInstance));
        });
      })
      .catch(console.error);
  };

  p5.setup = () => {
    const canvas = p5.createCanvas(p5.windowWidth, p5.windowHeight);

    p5.frameRate(30);

    document.oncontextmenu = function () {
      const coords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      //console.log("Determine if should display context menu: " + coords.x + ", " + coords.y)
      if (gridInstance.isOnGrid(coords.x, coords.y)) {
        //console.log("Should not display context menu: Is on grid!")
        return false;
      }
    };

    canvas.elt.addEventListener("wheel", onZoomScroll);

    canvas.elt.addEventListener("mousewheel", onZoomScroll);
  };

  p5.draw = () => {
    p5.background(220);
    p5.push();
    gridInstance.draw();
    mouseFixture?.draw(gridInstance.size);
    p5.pop();
  };

  p5.windowResized = () => {
    p5.resizeCanvas(p5.windowWidth, p5.windowHeight);
    gridInstance.resize();
  };

  p5.mouseWheel = (event) => {
    if (event.originalTarget.tagName !== "CANVAS") return;

    if (event.delta > 0) {
      gridInstance.scale += 0.1;
    } else {
      gridInstance.scale -= 0.1;
    }

    gridInstance.scale = Math.max(0.1, gridInstance.scale);
    gridInstance.resize();
  };

  p5.mousePressed = () => {
    if (window.grid.state === "place") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
          mouseFixture = rack;
        }
      }
    } else if (window.grid.state === "erase") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
        }
      }
    } else {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) rack.COLOR = window.grid.paint.COLOR;
    }
  };

  p5.mouseDragged = async () => {
    if (window.grid.state === "paint") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        rack.COLOR = window.grid.paint.COLOR;
        rack.SUPERCATEGORY_TUID = window.grid.paint.SUPERCATEGORY_TUID;

        paintAggregator(rack);
      }
    } else if (window.grid.state === "erase") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
        }
      }
    } else if (window.grid.state === "employee_area_paint") {
      const { x, y } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const isOnGrid = gridInstance.isOnGrid(x, y);

      if (isOnGrid) {
        const newEmployeeArea = new EmployeeArea(p5, floorsetId, x, y);
        addEmployeeAreaPaintAggregator(newEmployeeArea.toObject());
      }
    } else if (window.grid.state === "employee_area_erase") {
      const { x, y } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const isOnGrid = gridInstance.isOnGrid(x, y);

      if (isOnGrid) {
        const newEmployeeArea = new EmployeeArea(p5, floorsetId, x, y);
        deleteEmployeeAreaPaintAggregator(newEmployeeArea.toObject());
      }
    } else {
      if (mouseFixture) {
        const { x: gridX, y: gridY } = gridInstance.toGridCoordinates(
          p5.mouseX,
          p5.mouseY
        );
        if (
          gridX < 0 ||
          gridX > gridInstance.x ||
          gridY < 0 ||
          gridY > gridInstance.y
        )
          return;
        mouseFixture.X_POS = gridX;
        mouseFixture.Y_POS = gridY;
      }
      // } else if (window.draggedFixture) {
      //     const { width, height } = window.draggedFixture;
      //     const { x: gridX, y: gridY } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      //     if (gridX + width > gridInstance.x || gridY + height > gridInstance.y) return;
      //     mouseFixture = new Fixture(p5,gridInstance.fixtures.length + 1,  gridX, gridY, width, height);
      // }
    }
  };

  p5.mouseReleased = () => {
    console.log(window.draggedFixture, mouseFixture);

    if (mouseFixture) {
      updateFixtureInstance(mouseFixture)
        .then(() => {
          console.log(mouseFixture);
          gridInstance.fixtures.push(mouseFixture);
          mouseFixture = undefined;
          window.draggedFixture = undefined;
        })
        .catch(console.error);
    } else if (window.draggedFixture) {
      const { x, y } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);

      console.log({
        ...window.draggedFixture,
        COLOR: "#fff",
        FLOORSET_TUID: floorsetId,
        X_POS: x,
        Y_POS: y,
        ALLOCATED_LF: 1,
        EDITOR_ID: gridInstance.fixtures.length + 1,
      });
      createFixtureInstance(
        Fixture.from(p5, {
          ...window.draggedFixture,
          COLOR: "#fff",
          FLOORSET_TUID: floorsetId,
          X_POS: x,
          Y_POS: y,
          ALLOCATED_LF: 1,
          EDITOR_ID: gridInstance.fixtures.length + 1,
        })
      )
        .then((data) => {
          console.log(data);
          gridInstance.fixtures.push(
            Fixture.from(p5, {
              ...window.draggedFixture,
              COLOR: "#fff",
              FLOORSET_TUID: floorsetId,
              X_POS: x,
              Y_POS: y,
              ALLOCATED_LF: 1,
              EDITOR_ID: gridInstance.fixtures.length + 1,
            })
          );
          mouseFixture = undefined;
          window.draggedFixture = undefined;
        })
        .catch(console.error);
    }
  };
}

//Test function to save the canvas as an image
// document.addEventListener("keypress", (event) => {
//   if (event.keyCode == 83) {
//     // "S" Key
//     p5Instance.saveCanvas("floorsetGrid", "jpg");
//     console.log("Saving Image!");
//   }
// });

window.initP5 = (elementId) => {
  // Check if p5 is already rendered to prevent
  // ghosting issues when rendering.
  if (window.p5Instance) {
    window.p5Instance.remove();
    window.p5Instance = null;
  }
  new p5(sketch, elementId);
};

window.addFixture = (id, x, y, width, length) => {
  if (!window.gridInstance) {
    console.error("Grid not initialized yet!");
    return;
  }

  window.gridInstance.addFixtureInstanceToGrid(id, x, y, width, length);
};

window.updateStoreSize = (width, height) => {
  if (!window.gridInstance) {
    console.error("Grid not initialized yet!");
    return;
  }

  window.gridInstance.width = width;
  window.gridInstance.height = height;
  window.gridInstance.resize();
};

window.setPaintMode = (enabled) => {
  console.log("Setting paint mode " + enabled);
  //Translate true/false to "paint" / "place"
  if (enabled) {
    window.grid.state = "paint";
  } else {
    window.grid.state = "place";
  }
  console.log("window gridstate now: " + window.grid.state);
};

window.setErase = () => {
  window.grid.paint.COLOR = "#fff";
  window.grid.paint.SUPERCATEGORY_TUID = 0;
  window.grid.state = "paint";
};

window.setPlace = () => {
  window.grid.state = "place";
};

window.setEmployeeAreaPaint = () => {
  window.grid.state = "employee_area_paint";
};

window.setEmployeeAreaErase = () => {
  window.grid.state = "employee_area_erase";
};

window.setPaint = (paint, supercategory_tuid) => {
  window.grid.paint.COLOR = paint;
  window.grid.paint.SUPERCATEGORY_TUID = supercategory_tuid;
};

window.createDraggable = (event) => {
  const WIDTH = Number(event.target.getAttribute("data-width")),
    LENGTH = Number(event.target.getAttribute("data-height")),
    NAME = String(event.target.getAttribute("data-name")),
    FIXTURE_TUID = Number(event.target.getAttribute("data-fixture-tuid")),
    STORE_TUID = Number(event.target.getAttribute("data-store-tuid"));

  window.draggedFixture = { WIDTH, LENGTH, NAME, FIXTURE_TUID, STORE_TUID };
  console.log(window.draggedFixture);
};

//This method is used with the save button it copies the current floorset, creates another
// grid and canvas, scales the image so the whole floorset is shown and
// renders the copied floorset outside of the users visible UI. The image
// created is used for the floorsets dashboard.- Michael Polhill
window.captureFloorsetThumbnail = async () => {
  return await new Promise((resolve, reject) => {
    //Ensure grid is rendered
    if (!window.gridInstance || !window.gridInstance.fixtures) {
      console.error("gridInstance or fixtures not available");
      reject("error page not rendered");
      return;
    }

    // Resolution for the thumbnail.
    // 300~ kept breaking the blazor SignalR
    // websocket so its set to 200. It works well.
    const width = 200;
    const height = 200;

    //New p5 to render off screen
    const sketch = (p5) => {
      p5.setup = () => {
        p5.createCanvas(width, height);
        p5.background(255);

        //Create a new grid and copy the primary grid dimensions
        const grid = new Grid(p5);
        grid.width = window.gridInstance.width;
        grid.height = window.gridInstance.height;
        // Set scale so entire floor plan will be in the image
        grid.scale = Math.min(
          width / (grid.width * grid.size),
          height / (grid.height * grid.size)
        );
        grid.resize();

        //Clone fixtures on to the new grid
        grid.fixtures = window.gridInstance.fixtures.map((f) => {
          const fixture = Fixture.from(p5, f);
          return fixture;
        });

        // Render the new floorset
        p5.push();
        grid.draw();
        p5.pop();

        // Wait for render, then capture image
        setTimeout(() => {
          const dataUrl = p5.canvas.toDataURL("image/png");
          resolve(dataUrl);
          p5.remove(); //Clean up the grid
        }, 100);
      };
    };

    // Hidden container off screen to hold the
    // copied grid.
    const container = document.createElement("div");
    container.style.position = "absolute";
    container.style.left = "-9999px";
    container.style.top = "-9999px";
    document.body.appendChild(container);

    new p5(sketch, container);
  });
};
