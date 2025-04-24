/**
 * Create a new employee area instance
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class EmployeeArea {
  static p5;

  /**
   * @property {number} FLOORSET_TUID
   * @property {number} X_POS
   * @property {number} Y_POS
   */
  constructor(FLOORSET_TUID, X_POS, Y_POS) {
    this.FLOORSET_TUID = FLOORSET_TUID;
    this.X_POS = X_POS;
    this.Y_POS = Y_POS;
  }

  draw(gridSize) {
    EmployeeArea.p5.push();
    EmployeeArea.p5.fill("red");
    // EmployeeArea.p5.stroke(0);
    // EmployeeArea.p5.strokeWeight(3);
    EmployeeArea.p5.rect(
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      gridSize,
      gridSize
    );
    EmployeeArea.p5.pop();
  }

  toObject() {
    return {
      FLOORSET_TUID: this.FLOORSET_TUID,
      X_POS: this.X_POS,
      Y_POS: this.Y_POS,
    };
  }

  /**
   * @param {Object} object
   */
  static from(object) {
    return new EmployeeArea(object.FLOORSET_TUID, object.X_POS, object.Y_POS);
  }
}

export default EmployeeArea;
