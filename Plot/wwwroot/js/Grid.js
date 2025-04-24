import Fixture from "./Fixture.js";
import EmployeeArea from "./EmployeeArea.js";

/**
 * Create a new grid
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Grid {
  static p5;

  /** @type {Fixture[]} */
  fixtures;

  /** @type {Map<string, EmployeeArea>} */
  employeeAreas;

  /**
   * @param {number} size
   * @param {number} scale
   * @param {number} width
   * @param {number} height
   * @param {p5.Color | string} color
   */
  constructor() {
    this.size = 30;
    this.scale = 1;
    this.xOffset = 0;
    this.yOffset = 0;
    this.fixtures = [];
    this.employeeAreas = new Map();
    this.width = 1;
    this.height = 1;
    this.resize();
  }

  addFixtureInstanceToGrid(...args) {
    this.fixtures.push(new Fixture(...args));
  }

  /**
   * TODO: Write documentation
   * @param {EmployeeArea[]} employeeAreas
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
   * TODO: Write documentation
   * @param {number} floorsetTuid
   * @param {number} x1
   * @param {number} y1
   * @param {number} x2
   * @param {number} y2
   */
  bulkAddEmployeeAreas(floorsetTuid, x1, y1, x2, y2) {
    for (let y = y1; y < y2; y++) {
      for (let x = x1; x < x2; x++) {
        this.employeeAreas.set(
          [x, y].join("-"),
          new EmployeeArea(floorsetTuid, x, y)
        );
      }
    }
  }

  /**
   * TODO: Write documentation
   * @param {EmployeeArea[]} employeeAreas
   */
  deleteEmployeeAreas(employeeAreas) {
    employeeAreas.forEach((employeeArea) =>
      this.employeeAreas.delete(
        [employeeArea.X_POS, employeeArea.Y_POS].join("-")
      )
    );
  }

  /**
   * TODO: Write documentation
   * @param {number} floorsetTuid
   * @param {number} x1
   * @param {number} y1
   * @param {number} x2
   * @param {number} y2
   */
  bulkDeleteEmployeeAreas(x1, y1, x2, y2) {
    for (let y = y1; y < y2; y++) {
      for (let x = x1; x < x2; x++) {
        this.employeeAreas.delete([x, y].join("-"));
      }
    }
  }

  toGridCoordinates(v) {
    const scaleSize = this.size * this.scale;
    const x = Math.floor((v.x - this.translate.x) / scaleSize);
    const y = Math.floor((v.y - this.translate.y) / scaleSize);
    return Grid.p5.createVector(x, y);
  }

  screenToGridSpace(x, y) {
    return {
      x: (x - this.translate.x) / (this.size * this.scale),
      y: (y - this.translate.y) / (this.size * this.scale),
    };
  }

  isOnGrid(x, y) {
    if (x >= 0 && x < this.width && y >= 0 && y < this.height) {
      return true;
    }
    return false;
  }

  deleteFixtureByTuid(tuid) {
    this.fixtures = this.fixtures.filter((fixture) => fixture.TUID != tuid);
  }

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

  draw(mouseFixture) {
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

    this.fixtures.forEach((rack) => rack.draw(this.size));

    mouseFixture?.draw(this.size);

    Grid.p5.pop();
  }
}

export default Grid;
