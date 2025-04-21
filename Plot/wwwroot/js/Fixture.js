/**
 * Create a new fixture instance
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Fixture {
  /**
   * @property {number} TUID
   * @property {number} EDITOR_ID
   * @property {string} NAME
   * @property {number} FIXTURE_TUID
   * @property {number} FLOORSET_TUID
   * @property {number} X_POS
   * @property {number} Y_POS
   * @property {string} COLOR
   * @property {number} LENGTH
   * @property {number} WIDTH
   * @property {number} HANGER_STACK
   * @property {number} SUPERCATEGORY_TUID
   * @property {string} SUBCATEGORY
   * @property {number} SUBCATEGORY_NAME
   * @property {string} NOTE
   * @property {number} ALLOCATED_LF
   */
  constructor(
    p5,
    TUID,
    EDITOR_ID,
    NAME,
    FIXTURE_TUID,
    FLOORSET_TUID,
    X_POS,
    Y_POS,
    COLOR = "#fff",
    LENGTH,
    WIDTH,
    HANGER_STACK = 1,
    SUPERCATEGORY_TUID = 0,
    SUBCATEGORY,
    SUBCATEGORY_NAME,
    NOTE,
    ALLOCATED_LF = 0
  ) {
    this.p5 = p5;
    this.TUID = TUID;
    this.EDITOR_ID = EDITOR_ID;
    this.NAME = NAME;
    this.FIXTURE_TUID = FIXTURE_TUID;
    this.FLOORSET_TUID = FLOORSET_TUID;
    this.X_POS = X_POS;
    this.Y_POS = Y_POS;
    this.LENGTH = LENGTH;
    this.WIDTH = WIDTH;
    this.HANGER_STACK = HANGER_STACK;
    this.SUPERCATEGORY_TUID = SUPERCATEGORY_TUID;
    this.SUBCATEGORY = SUBCATEGORY;
    this.SUBCATEGORY_NAME = SUBCATEGORY_NAME;
    this.NOTE = NOTE;
    this.ALLOCATED_LF = ALLOCATED_LF;
    this.COLOR = COLOR;
  }

  draw(gridSize) {
    this.p5.push();
    this.p5.fill(this.COLOR);
    this.p5.stroke(0);
    this.p5.strokeWeight(3);
    this.p5.rect(
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );
    this.p5.pop();
  }

  toObject() {
    return {
      TUID: this.TUID,
      EDITOR_ID: this.EDITOR_ID,
      NAME: this.NAME,
      FIXTURE_TUID: this.FIXTURE_TUID,
      FLOORSET_TUID: this.FLOORSET_TUID,
      X_POS: this.X_POS,
      Y_POS: this.Y_POS,
      LENGTH: this.LENGTH,
      WIDTH: this.WIDTH,
      HANGER_STACK: this.HANGER_STACK,
      SUPERCATEGORY_TUID: this.SUPERCATEGORY_TUID,
      SUBCATEGORY: this.SUBCATEGORY,
      SUBCATEGORY_NAME: this.SUBCATEGORY_NAME,
      NOTE: this.NOTE,
      ALLOCATED_LF: this.ALLOCATED_LF,
      COLOR: this.COLOR,
    };
  }

  /**
   * @param {Object} object
   */
  static from(p5, object) {
    return new Fixture(
      p5,
      object.TUID,
      object.EDITOR_ID,
      object.NAME,
      object.FIXTURE_TUID,
      object.FLOORSET_TUID,
      object.X_POS,
      object.Y_POS,
      object.COLOR,
      object.LENGTH,
      object.WIDTH,
      object.HANGER_STACK,
      object.SUPERCATEGORY_TUID,
      object.SUBCATEGORY,
      object.SUBCATEGORY_NAME,
      object.NOTE,
      object.ALLOCATED_LF
    );
  }
}

export default Fixture;
