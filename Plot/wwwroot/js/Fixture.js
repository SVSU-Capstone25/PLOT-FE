/**
 * Create a new Fixture instance representing a store rack, shelf, or similar display object.
 * Used for rendering and tracking layout items on a grid.
 *
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
class Fixture {
  /** Static property to hold a reference to the p5.js instance for drawing */
  static p5;

  /**
   * @property {number} TUID - Unique identifier for this fixture instance.
   * @property {string} NAME - Human-readable name of the fixture.
   * @property {number} FIXTURE_TUID - Template identifier for fixture type.
   * @property {number} FLOORSET_TUID - Associated floorset ID.
   * @property {number} X_POS - X coordinate (grid units).
   * @property {number} Y_POS - Y coordinate (grid units).
   * @property {string} COLOR - Color to fill the fixture when drawn.
   * @property {number} LENGTH - Fixture length (grid units).
   * @property {number} WIDTH - Fixture width (grid units).
   * @property {number} HANGER_STACK - Number of hanger stacks.
   * @property {number} SUPERCATEGORY_TUID - Supercategory ID.
   * @property {string} SUPERCATEGORY_NAME - Name of the supercategory.
   * @property {string} SUBCATEGORY - Name of the subcategory.
   * @property {string} NOTE - Additional notes about the fixture.
   * @property {number} FIXTURE_IDENTIFIER - Optional identifier for fixture linking.
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
    // Initialize fixture properties
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

  /**
   * Draw this fixture on a p5.js canvas.
   * @param {number} gridSize - The size of a single grid cell (pixels).
   * @param {boolean} isPrinted - Whether to render with print-specific formatting (smaller text).
   */
  draw(gridSize, isPrinted = false) {
    Fixture.p5.push(); // Save current drawing state

    // Draw the fixture rectangle
    Fixture.p5.fill(this.COLOR);
    Fixture.p5.stroke(0);
    Fixture.p5.strokeWeight(3);
    Fixture.p5.rect(
      this.X_POS * gridSize,
      this.Y_POS * gridSize,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );

    // Draw the fixture labels
    Fixture.p5.strokeWeight(0.5);
    Fixture.p5.fill("black");
    Fixture.p5.textSize(8);

    if (isPrinted) {
      // Displays fixture TUID
      Fixture.p5.textSize(4);
      Fixture.p5.textAlign(Fixture.p5.LEFT, Fixture.p5.TOP);
      Fixture.p5.text(
        this.TUID,
        this.X_POS * gridSize + 4,
        this.Y_POS * gridSize + 4,
        this.WIDTH * gridSize,
        this.LENGTH * gridSize
      );
    }

    // Draw hanger stack number at the top left
    Fixture.p5.textAlign(Fixture.p5.RIGHT, Fixture.p5.TOP);
    Fixture.p5.text(
      this.HANGER_STACK,
      this.X_POS * gridSize - 4,
      this.Y_POS * gridSize + 4,
      this.WIDTH * gridSize,
      this.LENGTH * gridSize
    );

    // Draw subcategory label at the center
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

    // Draw a shortened version of the note at the bottom
    Fixture.p5.strokeWeight(0.1);
    Fixture.p5.textSize(4);
    Fixture.p5.textAlign(Fixture.p5.LEFT, Fixture.p5.BOTTOM);
    Fixture.p5.text(
      `${(this.NOTE ?? "").substring(0, 12 * this.WIDTH)}...`,
      this.X_POS * gridSize + 4,
      this.Y_POS * gridSize - 4,
      this.WIDTH * gridSize - 4,
      this.LENGTH * gridSize
    );

    Fixture.p5.pop(); // Restore previous drawing state
  }

  /**
   * Convert the fixture instance to a plain object for serialization.
   * @returns {Object} Object representation of the fixture.
   */
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
   * Static method to create a Fixture instance from a plain object.
   * @param {Object} object - An object with the same structure as Fixture.
   * @returns {Fixture} A new Fixture instance.
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
