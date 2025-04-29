/**
 * Create a new employee area instance
 * Represents a 1x1 square area on the floorplan reserved for employees (e.g., backroom, breakroom)
 *
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class EmployeeArea {
  /** Static p5 reference to the p5 instance used for drawing */
  static p5;

  /**
   * @param {number} FLOORSET_TUID - Unique ID linking this area to a specific floorset
   * @param {number} X_POS - X-coordinate on the grid
   * @param {number} Y_POS - Y-coordinate on the grid
   */
  constructor(FLOORSET_TUID, X_POS, Y_POS) {
    this.FLOORSET_TUID = FLOORSET_TUID;
    this.X_POS = X_POS;
    this.Y_POS = Y_POS;
  }

  /**
   * Draw this employee area on the p5 canvas
   * @param {number} gridSize - Size of one grid cell in pixels
   */
  draw(gridSize) {
    EmployeeArea.p5.push();
    EmployeeArea.p5.fill("red");
    EmployeeArea.p5.rect(
      this.X_POS * gridSize, // X position in pixels
      this.Y_POS * gridSize, // Y position in pixels
      gridSize, // Width = 1 grid unit
      gridSize // Height = 1 grid unit
    );
    EmployeeArea.p5.pop();
  }

  /**
   * Convert instance to plain object (e.g., for serialization or storage)
   * @returns {Object} Plain object representing the employee area.
   */
  toObject() {
    return {
      FLOORSET_TUID: this.FLOORSET_TUID,
      X_POS: this.X_POS,
      Y_POS: this.Y_POS,
    };
  }

  /**
   * Create an employee area from a plain object (e.g., from database or JSON).
   * @param {Object} object - Object containing employee area data.
   * @returns {EmployeeArea} A new employee area instance.
   */
  static from(object) {
    return new EmployeeArea(object.FLOORSET_TUID, object.X_POS, object.Y_POS);
  }
}

export default EmployeeArea;
