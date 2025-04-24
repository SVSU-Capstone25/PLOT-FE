/**
 * Create a new fixture instance
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Fixture {
  static p5;

  /**
   * @property {number} TUID
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
   * @property {string} SUPERCATEGORY_NAME
   * @property {string} SUBCATEGORY
   * @property {string} NOTE
   * @property {number} FIXTURE_IDENTIFIER
   */
  constructor(
    TUID,
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
    SUPERCATEGORY_NAME,
    SUBCATEGORY,
    NOTE,
    FIXTURE_IDENTIFIER
  ) {
    this.TUID = TUID;
    this.NAME = NAME;
    this.FIXTURE_TUID = FIXTURE_TUID;
    this.FLOORSET_TUID = FLOORSET_TUID;
    this.X_POS = X_POS;
    this.Y_POS = Y_POS;
    this.LENGTH = LENGTH;
    this.WIDTH = WIDTH;
    this.HANGER_STACK = HANGER_STACK;
    this.SUPERCATEGORY_TUID = SUPERCATEGORY_TUID;
    this.SUPERCATEGORY_NAME = SUPERCATEGORY_NAME;
    this.SUBCATEGORY = SUBCATEGORY;
    this.NOTE = NOTE;
    this.COLOR = COLOR;
    this.FIXTURE_IDENTIFIER = FIXTURE_IDENTIFIER;
  }

  draw(gridSize) {
    Fixture.p5.push();
    Fixture.p5.fill(this.COLOR);
    Fixture.p5.stroke(0);
    Fixture.p5.strokeWeight(3);
    Fixture.p5.rect(
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );
    Fixture.p5.strokeWeight(1);
    Fixture.p5.fill("black");
    Fixture.p5.textSize(24);
    Fixture.p5.text(
      this.HANGER_STACK,
      this.X_POS * gridSize + this.WIDTH * gridSize - 24,
      this.Y_POS * gridSize + 8,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );
    Fixture.p5.strokeWeight(0.75);
    Fixture.p5.textSize(8);
    Fixture.p5.textAlign(Fixture.p5.CENTER, Fixture.p5.CENTER);
    Fixture.p5.text(
      this.SUBCATEGORY ?? "",
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );
    Fixture.p5.strokeWeight(0.1);
    Fixture.p5.textSize(4);
    Fixture.p5.textAlign(Fixture.p5.CENTER, Fixture.p5.BOTTOM);
    Fixture.p5.text(
      this.NOTE ?? "",
      this.X_POS * gridSize,
      this.Y_POS * gridSize - 4,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );
    Fixture.p5.pop();
  }

  toObject() {
    return {
      TUID: this.TUID,
      NAME: this.NAME,
      FIXTURE_TUID: this.FIXTURE_TUID,
      FLOORSET_TUID: this.FLOORSET_TUID,
      X_POS: this.X_POS,
      Y_POS: this.Y_POS,
      LENGTH: this.LENGTH,
      WIDTH: this.WIDTH,
      HANGER_STACK: this.HANGER_STACK,
      SUPERCATEGORY_TUID: this.SUPERCATEGORY_TUID,
      SUPERCATEGORY_NAME: this.SUPERCATEGORY_NAME,
      SUBCATEGORY: this.SUBCATEGORY,
      NOTE: this.NOTE,
      COLOR: this.COLOR,
      FIXTURE_IDENTIFIER: this.FIXTURE_IDENTIFIER,
    };
  }

  /**
   * @param {Object} object
   */
  static from(object) {
    return new Fixture(
      object.TUID,
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
      object.SUPERCATEGORY_NAME,
      object.SUBCATEGORY,
      object.NOTE,
      object.FIXTURE_IDENTIFIER
    );
  }
}

export default Fixture;
