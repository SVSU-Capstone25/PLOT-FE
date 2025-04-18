/**
 * Create a new employee area instance
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class EmployeeArea {
  /**
   * @property {number} FLOORSET_TUID
   * @property {number} X_POS
   * @property {number} Y_POS
   */
  constructor(p5, FLOORSET_TUID, X_POS, Y_POS) {
    this.p5 = p5;
    this.FLOORSET_TUID = FLOORSET_TUID;
    this.X_POS = X_POS;
    this.Y_POS = Y_POS;
  }

  draw(gridSize) {
    this.p5.push();
    this.p5.fill("red");
    this.p5.stroke(0);
    this.p5.strokeWeight(3);
    this.p5.rect(
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      gridSize,
      gridSize
    );
    this.p5.pop();
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
  static from(p5, object) {
    return new EmployeeArea(
      p5,
      object.FLOORSET_TUID,
      object.X_POS,
      object.Y_POS
    );
  }
}

export default EmployeeArea;
