import Fixture from "./Fixture.js";
import EmployeeArea from "./EmployeeArea.js";
import Grid from "./Grid.js";
import { getFixtureInstances, getEmployeeAreas } from "./getters.js";
import {
  updateFixtureInstance,
  createFixtureInstance,
  bulkCreateEmployeeAreas,
  bulkDeleteEmployeeAreas,
} from "./setters.js";

/**
 * @typedef {import('./globals.d.ts')}
 */

/** @type {Grid | null} */
let gridInstance;
let p5Instance;

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
  window.grid.state = enabled ? "paint" : "place";
};

window.setPaint = (paint, supercategory_tuid, subcategory) => {
  window.grid.paint.COLOR = paint;
  window.grid.paint.SUPERCATEGORY_TUID = supercategory_tuid;
  window.grid.paint.SUBCATEGORY = subcategory;
};

window.setErase = () => {
  window.grid.state = "erase";
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

window.createDraggable = (event) => {
  const WIDTH = Number(event.target.getAttribute("data-width")),
    LENGTH = Number(event.target.getAttribute("data-height")),
    NAME = String(event.target.getAttribute("data-name")),
    FIXTURE_TUID = Number(event.target.getAttribute("data-fixture-tuid")),
    STORE_TUID = Number(event.target.getAttribute("data-store-tuid"));

  window.draggedFixture = { WIDTH, LENGTH, NAME, FIXTURE_TUID, STORE_TUID };
};

// Andrew K, Andrew F
// This function toggles a menu on right click
// positioned at the given coordinates.
window.showDropdown = (isShowing, x = 0, y = 0) => {
  // Create bootstrap dropdown element 
  const dropdown = new bootstrap.Dropdown(
    document.querySelector("#fixtureContextMenu")
  );
  const menuWidth = 350;

  // If the click happens on the outer right 20% of the screen,
  // display the menu to the left of the cursor.
  // Otherwise, display it on the right side of the cursor.
  if (x < window.innerWidth - window.innerWidth * 0.2) {
    dropdown._element.style.left = x + 40 + "px";
  } else {
    dropdown._element.style.left = x - menuWidth - 40 + "px";
  }

  // If the click happens in the lower 50% of the screen,
  // display the menu above the cursor.
  // Otherwise, display the menu below the cursor.
  if (y < window.innerHeight - window.innerHeight * 0.5) {
    dropdown._element.style.top = y + "px";
  } else {
    dropdown._element.style.top = y - window.innerHeight * 0.4 + "px";
  }

  // Make the dropdown visible depending on the given boolean
  if (isShowing) {
    dropdown.show();
  } else {
    dropdown.hide();
  }
};

window.reload = (floorsetId) => {
  gridInstance.employeeAreas = new Map();
  getEmployeeAreas(floorsetId)
    .then((employeeAreas) => {
      employeeAreas.forEach((employeeArea) => {
        gridInstance.employeeAreas.set(
          [employeeArea.X_POS, employeeArea.Y_POS].join("-"),
          EmployeeArea.from(employeeArea)
        );
      });
    })
    .catch(console.error);

  gridInstance.fixtures = [];
  getFixtureInstances(floorsetId)
    .then((fixtureInstances) => {
      fixtureInstances.forEach((fixtureInstance) => {
        gridInstance.fixtures.push(Fixture.from(fixtureInstance));
      });
    })
    .catch(console.error);
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
          const fixture = Fixture.from(f);
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

/**
 * @template T
 * @param {number} wait Wait time
 * @param {(item: T) => Promise<void>} onFlush
 * @returns
 */
function createDebouncedAggregator(wait, onFlush) {
  /** @type {Set<T>} */
  const items = new Set();
  let timerId = null;

  return function add(item) {
    items.add(JSON.stringify(item));
    clearTimeout(timerId);
    timerId = setTimeout(() => {
      const snapshot = Array.from(items).map(JSON.parse);
      onFlush(snapshot);
      items.clear();
    }, wait);
  };
}

function drawEmployeeAreaSelector(p5, v1, v2) {
  if (!v1) return;
  if (!v2) return;

  p5.push();
  p5.noStroke();
  p5.fill("#ff000066");
  p5.rect(v1.x, v1.y, v2.x - v1.x, v2.y - v1.y);
  p5.pop();
}

function onKeyboardKey(p5) {
  if (p5.keyIsDown(p5.LEFT_ARROW)) {
    gridInstance.xOffset += 50 * gridInstance.scale;
  } else if (p5.keyIsDown(p5.RIGHT_ARROW)) {
    gridInstance.xOffset -= 50 * gridInstance.scale;
  }

  if (p5.keyIsDown(p5.UP_ARROW)) {
    gridInstance.yOffset += 50 * gridInstance.scale;
  } else if (p5.keyIsDown(p5.DOWN_ARROW)) {
    gridInstance.yOffset -= 50 * gridInstance.scale;
  }

  gridInstance.resize();
}

function onZoomScroll(event) {
  if (!gridInstance) return;

  const mouseX = event.clientX;
  const mouseY = event.clientY;

  const gridSpaceBeforeZoom = gridInstance.screenToGridSpace(mouseX, mouseY);

  if (event.shiftKey) {
    const zoomSensitivity = 0.001;
    const newScale = gridInstance.scale * (1 + event.deltaY * zoomSensitivity);

    gridInstance.scale = Math.max(0.1, Math.min(10, newScale));
    gridInstance.resize();

    const gridSpaceAfterZoom = gridInstance.screenToGridSpace(mouseX, mouseY);

    gridInstance.xOffset +=
      (gridSpaceAfterZoom.x - gridSpaceBeforeZoom.x) *
      gridInstance.size *
      gridInstance.scale;
    gridInstance.yOffset +=
      (gridSpaceAfterZoom.y - gridSpaceBeforeZoom.y) *
      gridInstance.size *
      gridInstance.scale;

    gridInstance.scale = Math.max(0.1, Math.min(10, newScale));
  } else {
    gridInstance.xOffset += event.deltaX * 0.6 * gridInstance.scale;
    gridInstance.yOffset += event.deltaY * 0.3 * gridInstance.scale;
  }

  gridInstance.resize();
  event.preventDefault();
}

function sketch(p5) {
  let mouseFixture, floorsetId, employeeAreaSelection;

  const paintAggregator = createDebouncedAggregator(500, (fixtures) => {
    console.log(fixtures);
    Promise.allSettled(
      fixtures.map((fixture) => updateFixtureInstance(fixture))
    )
      .then(() => {
        DotNet?.invokeMethodAsync("Plot", "UpdateAllocations");
      })
      .catch(console.error);
  });

  p5.preload = () => {
    Grid.p5 = p5;
    EmployeeArea.p5 = p5;
    Fixture.p5 = p5;
    p5Instance = p5;
    gridInstance = new Grid();
    window.grid = {};

    window.p5Instance = p5Instance;
    window.gridInstance = gridInstance;
    employeeAreaSelection = {
      v1: p5.createVector(0, 0),
      v2: p5.createVector(0, 0),
    };

    window.grid.state = "place";
    window.grid.paint = {
      COLOR: "#fff",
      SUPERCATEGORY_TUID: 0,
      SUBCATEGORY: "None",
    };

    const url = new URL(window.location.href);

    floorsetId = url.pathname
      .replace("/floorset-editor/", "")
      .split("/")
      .map(Number)[1];

    window.reload(floorsetId);
  };

  p5.reload = () => {
    gridInstance.employeeAreas = new Map();
    gridInstance.fixtures = [];

    p5.preload();
  };

  p5.setup = () => {
    const canvas = p5.createCanvas(p5.windowWidth, p5.windowHeight);

    p5.frameRate(15);

    canvas.elt.addEventListener("wheel", onZoomScroll);

    canvas.elt.addEventListener("mousewheel", onZoomScroll);

    canvas.elt.addEventListener("click", () => {
      window.showDropdown(false);
    });

    canvas.elt.addEventListener("contextmenu", (event) => {
      event.preventDefault();
      event.stopPropagation();
      return false;
    });
  };

  p5.draw = () => {
    p5.background(220);
    onKeyboardKey(p5);
    p5.push();
    gridInstance.draw(mouseFixture);
    drawEmployeeAreaSelector(
      p5,
      employeeAreaSelection.v1,
      employeeAreaSelection.v2
    );
    p5.pop();
  };

  p5.windowResized = () => {
    p5.resizeCanvas(p5.windowWidth, p5.windowHeight);
    gridInstance.resize();
  };

  p5.mousePressed = () => {
    const mouse = p5.createVector(p5.mouseX, p5.mouseY);

    if (p5.mouseButton === "left") {
      if (window.grid.state === "paint" || window.grid.state === "erase") {
        const { x, y } = gridInstance.toGridCoordinates(mouse);
        const rack = gridInstance.getFixtureAt(x, y);

        if (!rack) return;

        if (window.grid.state === "erase") {
          rack.COLOR = "#fff";
          rack.SUPERCATEGORY_TUID = 0;
          rack.SUBCATEGORY = "";
        } else {
          rack.COLOR = window.grid.paint.COLOR;
          rack.SUPERCATEGORY_TUID = window.grid.paint.SUPERCATEGORY_TUID;
          rack.SUBCATEGORY = window.grid.paint.SUBCATEGORY;
        }

        paintAggregator(rack.toObject());
      } else if (window.grid.state === "place") {
        const { x, y } = gridInstance.toGridCoordinates(mouse);
        const rack = gridInstance.getFixtureAt(x, y);

        if (!rack) return;

        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
          mouseFixture = rack;
        }
      } else if (
        window.grid.state === "employee_area_paint" ||
        window.grid.state === "employee_area_erase"
      ) {
        employeeAreaSelection.v1 = mouse;
        employeeAreaSelection.v2 = mouse;
      } else {
        const gridCoords = gridInstance.toGridCoordinates(mouse);
        const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
        if (rack) rack.COLOR = window.grid.paint.COLOR;
      }
    }

    if (p5.mouseButton === "right") {
      const { x, y } = gridInstance.toGridCoordinates(mouse);
      const rack = gridInstance.getFixtureAt(x, y);

      if (!rack) return;

      window.showDropdown(true, p5.mouseX, p5.mouseY);

      DotNet?.invokeMethodAsync(
        "Plot",
        "GetFixture",
        floorsetId,
        rack.TUID,
        rack.NAME,
        rack.HANGER_STACK,
        rack.SUBCATEGORY,
        rack.NOTE ?? ""
      );
    }
  };

  p5.mouseDragged = async () => {
    const mouse = p5.createVector(p5.mouseX, p5.mouseY);

    if (window.grid.state === "paint" || window.grid.state === "erase") {
      const { x, y } = gridInstance.toGridCoordinates(mouse);
      const rack = gridInstance.getFixtureAt(x, y);

      if (!rack) return;

      if (window.grid.state === "erase") {
        rack.COLOR = "#fff";
        rack.SUPERCATEGORY_TUID = 0;
        rack.SUBCATEGORY = "";
      } else {
        rack.COLOR = window.grid.paint.COLOR;
        rack.SUPERCATEGORY_TUID = window.grid.paint.SUPERCATEGORY_TUID;
        rack.SUBCATEGORY = window.grid.paint.SUBCATEGORY;
      }

      paintAggregator(rack.toObject());
    } else if (
      window.grid.state === "employee_area_paint" ||
      window.grid.state === "employee_area_erase"
    ) {
      employeeAreaSelection.v2 = mouse;
    } else {
      if (mouseFixture) {
        const { x, y } = gridInstance.toGridCoordinates(mouse);

        if (!gridInstance.isOnGrid(x, y)) return;
        if (
          !gridInstance.isOnGrid(
            x + mouseFixture.WIDTH - 1,
            y + mouseFixture.LENGTH - 1
          )
        )
          return;

        mouseFixture.X_POS = x;
        mouseFixture.Y_POS = y;
      }
    }
  };

  p5.mouseReleased = () => {
    if (window.grid.state === "employee_area_paint") {
      const v1 = gridInstance.toGridCoordinates(employeeAreaSelection.v1),
        v2 = gridInstance.toGridCoordinates(employeeAreaSelection.v2);

      const X1_POS = Math.min(v1.x, v2.x),
        Y1_POS = Math.min(v1.y, v2.y),
        X2_POS = Math.max(v1.x, v2.x),
        Y2_POS = Math.max(v1.y, v2.y);

      bulkCreateEmployeeAreas({
        FLOORSET_TUID: floorsetId,
        X1_POS,
        Y1_POS,
        X2_POS,
        Y2_POS,
      })
        .then(() => {
          gridInstance.bulkAddEmployeeAreas(
            floorsetId,
            X1_POS,
            Y1_POS,
            X2_POS,
            Y2_POS
          );
        })
        .catch(console.error);

      employeeAreaSelection.v1 = undefined;
      employeeAreaSelection.v2 = undefined;
      return;
    } else if (window.grid.state === "employee_area_erase") {
      const v1 = gridInstance.toGridCoordinates(employeeAreaSelection.v1),
        v2 = gridInstance.toGridCoordinates(employeeAreaSelection.v2);

      const X1_POS = Math.min(v1.x, v2.x),
        Y1_POS = Math.min(v1.y, v2.y),
        X2_POS = Math.max(v1.x, v2.x),
        Y2_POS = Math.max(v1.y, v2.y);

      bulkDeleteEmployeeAreas({
        FLOORSET_TUID: floorsetId,
        X1_POS,
        Y1_POS,
        X2_POS,
        Y2_POS,
      })
        .then(() => {
          gridInstance.bulkDeleteEmployeeAreas(X1_POS, Y1_POS, X2_POS, Y2_POS);
        })
        .catch(console.error);

      employeeAreaSelection.v1 = undefined;
      employeeAreaSelection.v2 = undefined;
      return;
    }

    if (mouseFixture) {
      updateFixtureInstance(mouseFixture.toObject())
        .then(() => {
          gridInstance.fixtures.push(mouseFixture);
          mouseFixture = undefined;
          window.draggedFixture = undefined;
        })
        .catch(console.error);
    } else if (window.draggedFixture) {
      const mouse = p5.createVector(p5.mouseX, p5.mouseY);
      const { x, y } = gridInstance.toGridCoordinates(mouse);

      if (!gridInstance.isOnGrid(x, y)) return;
      if (
        !gridInstance.isOnGrid(
          x + window.draggedFixture.WIDTH - 1,
          y + window.draggedFixture.LENGTH - 1
        )
      )
        return;

      createFixtureInstance(
        Fixture.from({
          ...window.draggedFixture,
          COLOR: "#fff",
          FLOORSET_TUID: floorsetId,
          X_POS: x,
          Y_POS: y,
        }).toObject()
      )
        .then((data) => {
          gridInstance.fixtures.push(
            Fixture.from({
              ...window.draggedFixture,
              TUID: data,
              COLOR: "#fff",
              FLOORSET_TUID: floorsetId,
              X_POS: x,
              Y_POS: y,
            })
          );
          mouseFixture = undefined;
          window.draggedFixture = undefined;
        })
        .catch(console.error);
    }
  };
}

window.initP5 = (elementId) => {
  // Check if p5 is already rendered to prevent
  // ghosting issues when rendering.
  if (window.p5Instance) {
    window.p5Instance.remove();
    window.p5Instance = null;
  }
  new p5(sketch, elementId);
};
