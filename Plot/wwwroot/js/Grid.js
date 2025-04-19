import Fixture from "./Fixture.js";
import EmployeeArea from "./EmployeeArea.js";

/**
 * Create a new grid
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Grid {
  /** @type {Map<string, EmployeeArea>} */
  employeeAreas;

  /**
   * @param {number} size
   * @param {number} scale
   * @param {number} width
   * @param {number} height
   * @param {p5.Color | string} color
   */
  constructor(p5) {
    this.p5 = p5;
    this.size = 30;
    this.scale = 1;
    this.fixtures = [];
    this.employeeAreas = new Map();
    this.width = 1;
    this.height = 1;
    this.resize();
  }

  addFixtureInstanceToGrid(...args) {
    this.fixtures.push(new Fixture(this.p5, ...args));
  }

  /**
   * TODO: Write documentation
   * @param {EmployeeArea[]} employeeAreas
   */
  addEmployeeAreas(employeeAreas) {
    employeeAreas.forEach((employeeArea) =>
      this.employeeAreas.set(
        [employeeArea.X_POS, employeeArea.Y_POS].join("-"),
        EmployeeArea.from(this.p5, employeeArea)
      )
    );
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

  toGridCoordinates(x, y) {
    return {
      x: Math.floor((x - this.translate.x) / (this.size * this.scale)),
      y: Math.floor((y - this.translate.y) / (this.size * this.scale)),
    };
  }

  isOnGrid(gridX, gridY) {
    if (gridX >= 0 && gridX < this.width && gridY >= 0 && gridY < this.height) {
      return true;
    }
    return false;
  }

  getFixtureAt(gridX, gridY) {
    for (const rack of this.fixtures) {
      const rackGridX = rack.X_POS;
      const rackGridY = rack.Y_POS;
      if (
        gridX >= rackGridX &&
        gridX < rackGridX + rack.WIDTH &&
        gridY >= rackGridY &&
        gridY < rackGridY + rack.LENGTH
      ) {
        return rack;
      }
    }
    return null;
  }

  resize() {
    this.translate = {
      x: this.p5.width / 2 - (this.size * this.width * this.scale) / 2,
      y: this.p5.height / 2 - (this.size * this.height * this.scale) / 2,
    };
  }

  draw() {
    this.p5.fill(255, 255, 255);
    this.p5.stroke(0, 100);
    this.p5.strokeWeight(1);
    this.p5.translate(this.translate.x, this.translate.y);
    this.p5.scale(this.scale);

    this.p5.rect(0, 0, this.width * this.size, this.height * this.size);

    for (let x = 1; x < this.width; x++) {
      this.p5.line(x * this.size, 0, x * this.size, this.height * this.size);
    }

    for (let y = 1; y < this.height; y++) {
      this.p5.line(0, y * this.size, this.width * this.size, y * this.size);
    }

    const employeeAreas = this.employeeAreas.values();

    for (const employeeArea of employeeAreas) {
      employeeArea.draw(this.size);
    }

    for (const rack of this.fixtures) {
      rack.draw(this.size);
    }
  }
}

export default Grid;
