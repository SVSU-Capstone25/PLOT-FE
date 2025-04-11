import Fixture from "./Fixture.js";
import Grid from "./Grid.js";
import { getFixtureInstances } from "./getters.js";
import { updateFixtureInstance, createFixtureInstance } from "./setters.js";

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

window.setPaintMode = (enabled) => {
  console.log("Setting paint mode " + enabled);
  //Translate true/false to "paint" / "place"
  if (enabled) {
    window.gridState = "paint";
  } else {
    window.gridState = "place";
  }
  console.log("window gridstate now: " + window.gridState);
}

function setErase() {
  window.paint = "#fff";
  window.gridState = "paint";
}

window.setPlace = () => {
  window.gridState = "place";
}

window.setPaint = (paint) => {
  console.log(paint);
  window.paint = paint;
}

// Tristan Calay 4/2/25 - Toggles for employee paint/erase mode.
// var isEmployeePaintEnabled = false;
// var isEmployeeEraseEnabled = false;

function setEmployeePaint(newPaint) {
  // isEmployeePaintEnabled = newPaint;
  // console.log("Employee paint mode is " + isEmployeePaintEnabled);
  //var marker = document.getElementById("employeePaintEnabledMarker");
  // if (isEmployeePaintEnabled) {
  //     console.log("Setting marker green...")
  //     //marker.style.color = "green";
  //     window.gridState = 'employeeMode';
  // }
  // else {
  //     console.log("Setting marker black...")
  //     //marker.style.color = "black";
  //     window.gridState = 'place';
  // }
}

function setEmployeeErase(newErase) {
  // isEmployeeEraseEnabled = newErase;
  // if (isEmployeeEraseEnabled) {
  //     window.gridState = 'employeeMode';
  // }
}

//Tristan Calay 4/7/25
//Remove a rack from the racks array by ID
function deleteFixtureByID(id) {
  const racks = this.fixtures;
  console.log("Called delete rack with ID: " + id);
  console.log("Fixtures: " + racks);
  for (var i = 0; i < racks.length; i++) {
    console.log("Checking index " + i);
    const rack = this.fixtures[i];
    if (rack.EDITOR_ID === id) {
      console.log("Fixture deleted: " + id);
      this.fixtures.splice(i, 1);
      return;
    }
  }
}

function sketch(p5) {
  let mouseFixture, floorsetId;

  p5.preload = () => {
    p5Instance = p5;
    gridInstance = new Grid(p5);

    window.p5Instance = p5Instance;
    window.gridInstance = gridInstance;

    window.gridState = "place";
    window.paint = "#fff";

    const url = new URL(window.location.href);

    floorsetId = url.pathname
      .replace("/floorset-editor/", "")
      .split("/")
      .map(Number)[1];

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
    p5.createCanvas(p5.windowWidth, p5.windowHeight);
    p5.frameRate(30);

    document.oncontextmenu = function () {
      const coords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      //console.log("Determine if should display context menu: " + coords.x + ", " + coords.y)
      if (gridInstance.isOnGrid(coords.x, coords.y)) {
        //console.log("Should not display context menu: Is on grid!")
        return false;
      }
    };
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
    if (event.delta > 0) {
      gridInstance.scale += 0.1;
    } else {
      gridInstance.scale -= 0.1;
    }

    gridInstance.scale = Math.max(0.1, gridInstance.scale);
    gridInstance.resize();
  };

  p5.mousePressed = () => {
    if (window.gridState === "place") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
          mouseFixture = rack;
        }
      }
    } else if (window.gridState === "erase") {
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
      if (rack) rack.COLOR = window.paint;
    }
  };

  p5.mouseDragged = () => {
    if (window.gridState === "paint") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) rack.COLOR = window.paint;
    } else if (window.gridState === "erase") {
      const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
      const rack = gridInstance.getFixtureAt(gridCoords.x, gridCoords.y);
      if (rack) {
        const index = gridInstance.fixtures.indexOf(rack);
        if (index > -1) {
          gridInstance.fixtures.splice(index, 1);
        }
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
    } else if(window.draggedFixture) {
        const { x, y } = gridInstance.toGridCoordinates(
            p5.mouseX,
            p5.mouseY
          );

        console.log({
            ...window.draggedFixture,
            COLOR: "#fff",
            FLOORSET_TUID: floorsetId,
            X_POS: x,
            Y_POS: y,
            ALLOCATED_LF: 1,
            EDITOR_ID: gridInstance.fixtures.length + 1
        })
        createFixtureInstance(Fixture.from(p5, {
            ...window.draggedFixture,
            COLOR: "#fff",
            FLOORSET_TUID: floorsetId,
            X_POS: x,
            Y_POS: y,
            ALLOCATED_LF: 1,
            EDITOR_ID: gridInstance.fixtures.length + 1
        })).then((data) => {
            console.log(data);
            gridInstance.fixtures.push(Fixture.from(p5, {
                ...window.draggedFixture,
                COLOR: "#fff",
                FLOORSET_TUID: floorsetId,
                X_POS: x,
                Y_POS: y,
                ALLOCATED_LF: 1,
                EDITOR_ID: gridInstance.fixtures.length + 1
            }));
            mouseFixture = undefined;
            window.draggedFixture = undefined;
        }).catch(console.error);
    }
  };
}

//Test function to save the canvas as an image
document.addEventListener("keypress", (event) => {
  if (event.keyCode == 83) {
    // "S" Key
    p5Instance.saveCanvas("floorsetGrid", "jpg");
    console.log("Saving Image!");
  }
});

function addFixtureOnLoad(id, x, y, width, length, color) {
  setTimeout(function () {
    let newFixture = new Fixture(p5Instance, x, y, width, length, id);
    newFixture.color = color;
    this.fixtures.push(newFixture);
  }, 500);
}

window.initP5 = (elementId) => {
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

window.createDraggable = (event) => {
  const WIDTH = Number(event.target.getAttribute("data-width")),
    LENGTH = Number(event.target.getAttribute("data-height")),
    NAME = String(event.target.getAttribute("data-name")),
    FIXTURE_TUID = Number(event.target.getAttribute("data-fixture-tuid")),
    STORE_TUID = Number(event.target.getAttribute("data-store-tuid"));
  // window.draggedFixture = { width, height, name };
  window.draggedFixture = { WIDTH, LENGTH, NAME, FIXTURE_TUID, STORE_TUID };
};
