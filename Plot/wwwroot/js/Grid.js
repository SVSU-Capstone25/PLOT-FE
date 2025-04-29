import Fixture from "./Fixture.js";
import EmployeeArea from "./EmployeeArea.js";

/**
 * Represents a grid layout for fixtures and employee areas.
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Grid {
  /** Static property to hold a reference to the p5.js instance for drawing */
  static p5;

  /** @type {Fixture[]} */
  fixtures;

  /** @type {Map<string, EmployeeArea>} */
  employeeAreas;

  /**
   * Create a new Grid instance.
   */
  constructor() {
    this.size = 32;
    this.scale = 1;
    this.xOffset = 0;
    this.yOffset = 0;
    this.fixtures = [];
    this.employeeAreas = new Map();
    this.width = 1;
    this.height = 1;
    this.resize();
  }

  /**
   * Add a new fixture instance to the grid.
   * @param {...any} args - Arguments passed to the Fixture constructor.
   */
  addFixtureInstanceToGrid(...args) {
    this.fixtures.push(new Fixture(...args));
  }

  /**
   * Add multiple employee areas to the grid.
   * @param {EmployeeArea[]} employeeAreas - The employee areas to add.
   */
  addEmployeeAreas(employeeAreas) {
    employeeAreas.forEach((employeeArea) =>
      this.employeeAreas.set(
        [employeeArea.X_POS, employeeArea.Y_POS].join("-"),
        EmployeeArea.from(employeeArea)
      )
    );
  }

  /**
   * Add a rectangular block of employee areas to the grid.
   * @param {number} floorsetTuid - The floorset identifier.
   * @param {number} x1 - Starting x-coordinate.
   * @param {number} y1 - Starting y-coordinate.
   * @param {number} x2 - Ending x-coordinate (exclusive).
   * @param {number} y2 - Ending y-coordinate (exclusive).
   */
  bulkAddEmployeeAreas(floorsetTuid, x1, y1, x2, y2) {
    for (let y = Math.max(0, y1); y < Math.min(this.height, y2); y++) {
      for (let x = Math.max(0, x1); x < Math.min(this.width, x2); x++) {
        this.employeeAreas.set(
          [x, y].join("-"),
          new EmployeeArea(floorsetTuid, x, y)
        );
      }
    }
  }

  /**
   * Delete multiple employee areas from the grid.
   * @param {EmployeeArea[]} employeeAreas - The employee areas to delete.
   */
  deleteEmployeeAreas(employeeAreas) {
    employeeAreas.forEach((employeeArea) =>
      this.employeeAreas.delete(
        [employeeArea.X_POS, employeeArea.Y_POS].join("-")
      )
    );
  }

  /**
   * Delete a rectangular block of employee areas from the grid.
   * @param {number} x1 - Starting x-coordinate.
   * @param {number} y1 - Starting y-coordinate.
   * @param {number} x2 - Ending X coordinate (exclusive).
   * @param {number} y2 - Ending Y coordinate (exclusive).
   */
  bulkDeleteEmployeeAreas(x1, y1, x2, y2) {
    for (let y = Math.max(0, y1); y < Math.min(this.height, y2); y++) {
      for (let x = Math.max(0, x1); x < Math.min(this.width, x2); x++) {
        this.employeeAreas.delete([x, y].join("-"));
      }
    }
  }

  /**
   * Convert a screen vector to grid coordinates.
   * @param {p5.Vector} v - The vector to convert.
   * @returns {p5.Vector} - The corresponding grid coordinates.
   */
  toGridCoordinates(v) {
    const scaleSize = this.size * this.scale;
    const x = Math.floor((v.x - this.translate.x) / scaleSize);
    const y = Math.floor((v.y - this.translate.y) / scaleSize);
    return Grid.p5.createVector(x, y);
  }

  /**
   * Convert screen space coordinates to grid space.
   * @param {number} x - The screen x-coordinate.
   * @param {number} y - The screen y-coordinate.
   * @returns {{x: number, y: number}} - The corresponding grid space coordinates.
   */
  screenToGridSpace(x, y) {
    return {
      x: (x - this.translate.x) / (this.size * this.scale),
      y: (y - this.translate.y) / (this.size * this.scale),
    };
  }

  /**
   * Check if a grid position is within the bounds of the grid.
   * @param {number} x - The grid x-coordinate.
   * @param {number} y - The grid y-coordinate.
   * @returns {boolean} True if the position is on the grid, otherwise false.
   */
  isOnGrid(x, y) {
    return x >= 0 && x < this.width && y >= 0 && y < this.height;
  }

  /**
   * Delete a fixture from the grid by its TUID.
   * @param {number} tuid - The unique identifier of the fixture.
   */
  deleteFixtureByTuid(tuid) {
    this.fixtures = this.fixtures.filter((fixture) => fixture.TUID != tuid);
  }

  /**
   * Update the properties of a fixture by its TUID.
   * @param {number} tuid - The unique identifier of the fixture.
   * @param {number} hangerStack - The hanger stack value.
   * @param {string} subcategory - The subcategory name.
   * @param {number} supercategoryTuid - The supercategory TUID.
   * @param {p5.Color | string} supercategoryColor - The color for the supercategory.
   * @param {string} note - Additional notes.
   */
  updateFixtureByTuid(
    tuid,
    hangerStack,
    subcategory,
    supercategoryTuid,
    supercategoryColor,
    note
  ) {
    this.fixtures = this.fixtures.map((fixture) => {
      if (fixture.TUID == tuid) {
        fixture.HANGER_STACK = hangerStack;
        fixture.SUBCATEGORY = subcategory;
        fixture.SUPERCATEGORY_TUID = supercategoryTuid;
        fixture.COLOR = supercategoryColor;
        fixture.NOTE = note;
      }
      return fixture;
    });
  }

  /**
   * Get the fixture located at a specific grid position.
   * @param {number} gridX - The grid x-coordinate.
   * @param {number} gridY - The grid y-coordinate.
   * @returns {Fixture | null} - The fixture at the location, or null if none exists.
   */
  getFixtureAt(gridX, gridY) {
    for (const fixture of this.fixtures) {
      const x = fixture.X_POS;
      const y = fixture.Y_POS;
      if (
        gridX >= x &&
        gridX < x + fixture.WIDTH &&
        gridY >= y &&
        gridY < y + fixture.LENGTH
      ) {
        return fixture;
      }
    }
    return null;
  }

  /**
   * Recalculate the translation values for centering the grid.
   */
  resize() {
    this.translate = {
      x:
        Grid.p5.width / 2 -
        (this.size * this.width * this.scale) / 2 +
        this.xOffset,
      y:
        Grid.p5.height / 2 -
        (this.size * this.height * this.scale) / 2 +
        this.yOffset,
    };
  }

  /**
   * Draw the grid, fixtures, and employee areas.
   * @param {Fixture|null} mouseFixture - The fixture currently being placed (optional).
   * @param {boolean} [isBeingPrinted=false] - Whether the grid is being printed.
   * @param {Function | undefined} saveCallback - Callback to run after drawing.
   */
  draw(mouseFixture, isBeingPrinted = false, saveCallback = undefined) {
    Grid.p5.push();
    Grid.p5.fill(255, 255, 255);
    Grid.p5.stroke(0, 100);
    Grid.p5.strokeWeight(1);
    Grid.p5.translate(this.translate.x, this.translate.y);
    Grid.p5.scale(this.scale);

    Grid.p5.rect(0, 0, this.width * this.size, this.height * this.size);

    for (let x = 1; x < this.width; x++) {
      Grid.p5.line(x * this.size, 0, x * this.size, this.height * this.size);
    }

    for (let y = 1; y < this.height; y++) {
      Grid.p5.line(0, y * this.size, this.width * this.size, y * this.size);
    }

    const employeeAreas = this.employeeAreas.values();
    for (const employeeArea of employeeAreas) {
      employeeArea.draw(this.size);
    }

    this.fixtures.forEach((rack) => rack.draw(this.size, isBeingPrinted));
    mouseFixture?.draw(this.size);

    Grid.p5.pop();
    saveCallback?.();
  }

  /**
   * Print the grid layout to an image file.
   * @param {string} floorsetName - The name to use for the saved file.
   */
  print(floorsetName) {
    const gridPixelWidth = this.width * this.size;
    const gridPixelHeight = this.height * this.size;

    const scaleX = Grid.p5.width / gridPixelWidth;
    const scaleY = Grid.p5.height / gridPixelHeight;

    this.scale = Math.min(scaleX, scaleY);

    this.xOffset = 0;
    this.yOffset = 0;
    this.resize();

    Grid.p5.pixelDensity(8);
    this.draw(null, true, () => {
      Grid.p5.saveCanvas(`${floorsetName} - Floorset Image`, "jpg");
      Grid.p5.pixelDensity(1);
    });
  }

  /**
   * Generate a thumbnail image of the current grid layout.
   * @returns {Promise<string>} A promise that resolves with a base64-encoded image string.
   */
  async getImageThumbnail() {
    const gridPixelWidth = this.width * this.size;
    const gridPixelHeight = this.height * this.size;

    const scaleX = Grid.p5.width / gridPixelWidth;
    const scaleY = Grid.p5.height / gridPixelHeight;

    this.scale = Math.min(scaleX, scaleY);

    this.xOffset = 0;
    this.yOffset = 0;
    this.resize();

    Grid.p5.pixelDensity(0.25);

    this.draw(null, true);

    return new Promise((resolve) => {
      requestAnimationFrame(() => {
        const imageBase64 = Grid.p5.canvas.toDataURL("image/png");

        this.resize();
        this.draw();
        Grid.p5.pixelDensity(1);

        resolve(imageBase64);
      });
    });
  }
}

export default Grid;
