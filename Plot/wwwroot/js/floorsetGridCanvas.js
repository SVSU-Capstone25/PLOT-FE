function togglePaint() {
    window.gridState = window.gridState === 'paint' ? 'place' : 'paint';
}

function setErase() {
    window.paint = '#fff';
    window.gridState = 'paint';
}

function setPlace() {
    window.gridState = 'place';
}

function setPaint(paint) {
    console.log(paint);
    window.paint = paint;
}

const floorsetGrid = (function () {
    let sketchInstance = null;

    //Flag to make sure rack creation is called only once.
    let rackCreated = false;
    class Rack {
        constructor(sketch, x, y, width, height, id) {
            this.sketch = sketch;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = this.sketch.color(255, 255, 255);
            this.id = id;
        }

        draw(gridSize) {
            this.sketch.fill(this.color);
            this.sketch.stroke(0);
            this.sketch.strokeWeight(3);
            this.sketch.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
        }

        toString() {
            return "Rack " + this.id + ", W " + this.width + " / H " + this.height;
        }
    }

    class Grid {
        constructor(sketch) {
            this.sketch = sketch;
            this.size = 30;
            this.racks = [];
            this.scale = 1;
            this.resize();
        }

        get x() {
            return window.gridWidth ?? 10;
        }

        get y() {
            return window.gridHeight ?? 10;
        }

        getNextID() {
            if (this.racks.length <= 0) {
                return 0;
            }
            //Return the next id in the sequence based on the last ID in the array, plus 1.
            return this.racks[this.racks.length - 1].id + 1;
        }

        toGridCoordinates(x, y) {
            return {
                x: Math.floor((x - this.translate.x) / (this.size * this.scale)),
                y: Math.floor((y - this.translate.y) / (this.size * this.scale)),
            };
        }

        getRackAt(gridX, gridY) {
            for (const rack of this.racks) {
                const rackGridX = Math.floor(rack.x / this.size);
                const rackGridY = Math.floor(rack.y / this.size);
                if (
                    gridX >= rackGridX &&
                    gridX < rackGridX + rack.width &&
                    gridY >= rackGridY &&
                    gridY < rackGridY + rack.height
                ) {
                    return rack;
                }
            }
            return null;
        }

        resize() {
            this.translate = {
                x: this.sketch.width / 2 - (this.size * this.x * this.scale) / 2,
                y: this.sketch.height / 2 - (this.size * this.y * this.scale) / 2,
            };
        }

        draw() {
            this.sketch.fill(255, 255, 255);
            this.sketch.stroke(0, 100);
            this.sketch.strokeWeight(1);
            this.sketch.translate(this.translate.x, this.translate.y);
            this.sketch.scale(this.scale);
            for (let y = 0; y < this.y; y++) {
                for (let x = 0; x < this.x; x++) {
                    const x1 = x * this.size,
                        y1 = y * this.size;
                    this.sketch.rect(x1, y1, this.size, this.size);
                }
            }

            for (const rack of this.racks) {
                rack.draw(this.size);
            }
        }
    }

    return {
        init() {
            sketchInstance = new p5((sketch) => {
                let grid, mouseRack;

                sketch.setup = () => {
                    const $GRIDAREA = document.querySelector("div#grid-area");
                    if (!$GRIDAREA) return;

                    window.gridState = "place";


                    const canvas = sketch.createCanvas(sketch.windowWidth, sketch.windowHeight);
                    canvas.parent($GRIDAREA);
                    canvas.style("position", "absolute");
                    canvas.style("top", "0px");
                    canvas.style("left", "0px");

                    grid = new Grid(sketch);
                    window.paint = '#fff';
                };

                sketch.draw = () => {
                    sketch.background(220);
                    sketch.push();
                    grid.draw();
                    mouseRack?.draw(grid.size);
                    sketch.pop();
                };

                sketch.windowResized = () => {
                    sketch.resizeCanvas(sketch.windowWidth, sketch.windowHeight);
                    grid.resize();
                }

                sketch.mouseWheel = (event) => {
                    if (event.delta > 0) {
                        grid.scale += 0.1;
                    } else {
                        grid.scale -= 0.1;
                    }

                    grid.scale = Math.max(0.1, grid.scale);
                    grid.resize();
                }

                sketch.mousePressed = (event) => {
                    console.log(event)
                    const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                    const rack = grid.getRackAt(gridCoords.x, gridCoords.y);

                    if (rack) {
                        const index = grid.racks.indexOf(rack);
                        if (index > -1) {
                            switch (event.buttons) {
                                case 1:
                                    //Place, Erase, Paint modes
                                    switch (window.gridState) {
                                        case "place":
                                            if (mouseRack) return;
                                            grid.racks.splice(index, 1);
                                            mouseRack = rack;
                                            console.log("Clicked " + rack.toString())
                                            break;
                                        case "erase":
                                            grid.racks.splice(index, 1);
                                            break;
                                        case "paint":
                                            rack.color = window.paint;
                                            break;
                                    }
                                    break;

                                case 2:
                                    //Context Window Mode
                                    //console.log("Display context window of rack.")
                                    setSelectedFixture(rack.id);
                            }
                        }
                    }
                };

                sketch.mouseDragged = () => {
                    const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                    const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                    switch (window.gridState) {
                        case "paint":
                            if (rack) {
                                rack.color = window.paint;
                            }
                            break;
                        case "erase":
                            if (rack) {
                                const index = grid.racks.indexOf(rack);
                                if (index > -1) {
                                    grid.racks.splice(index, 1);
                                }
                            }
                            break;
                        case "place":
                            // Place mode logic
                            if (mouseRack) {
                                if (gridCoords.x < 0 || gridCoords.x + mouseRack.width > grid.x || gridCoords.y < 0 || gridCoords.y + mouseRack.height > grid.y) return;
                                console.log("Dragging mouse rack: " + mouseRack.x + ", " + mouseRack.y + ", ID " + mouseRack.id);
                                mouseRack.x = gridCoords.x * grid.size;
                                mouseRack.y = gridCoords.y * grid.size;
                            } else if (window.draggedRack) {
                                const { width, height, name } = window.draggedRack;
                                if (gridCoords.x + width > grid.x || gridCoords.y + height > grid.y) return;

                                console.log("Creating a new rack on drag event - coords " + gridCoords.x + ", " + gridCoords.y)

                                jsCreateNewFixture(name).then(id => {
                                    mouseRack = new Rack(sketch, gridCoords.x * grid.size, gridCoords.y * grid.size,
                                        width, height, id);
                                    console.log("Recieved ID " + id, " gridCoords is " + gridCoords.x + ", " + gridCoords.y)
                                    console.log(mouseRack.toString());
                                })
                            }
                            break;
                    }
                }

                sketch.mouseReleased = () => {
                    if (!mouseRack) return;

                    console.log("Pushing mouserack into racks...")
                    grid.racks.push(mouseRack);
                    mouseRack = undefined;
                    window.draggedRack = undefined;
                    //rackCreated = false;
                };
            }, document.querySelector("div#grid-area"));
        }
    };
})();

// Initialize when Blazor component loads
//floorsetGrid.init();

console.log("Floorset Grid Initialized...")