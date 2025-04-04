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

    class Rack {
        constructor(sketch, x, y, width, height) {
            this.sketch = sketch;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = this.sketch.color(255, 255, 255);
        }

        draw(gridSize) {
            this.sketch.fill(this.color);
            this.sketch.stroke(0);
            this.sketch.strokeWeight(3);
            this.sketch.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
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

                sketch.mousePressed = () => {
                    if (window.gridState === "place") {
                        const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                        const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                        if (rack) {
                            const index = grid.racks.indexOf(rack);
                            if (index > -1) {
                                grid.racks.splice(index, 1);
                                mouseRack = rack;
                            }
                        }
                    } else if (window.gridState === "erase") {
                        const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                        const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                        if (rack) {
                            const index = grid.racks.indexOf(rack);
                            if (index > -1) {
                                grid.racks.splice(index, 1);
                            }
                        }
                    } else {
                        const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                        const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                        if (rack) rack.color = window.paint;
                    }
                };

                sketch.mouseDragged = () => {
                    if (window.gridState === "paint") {
                        const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                        const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                        if (rack) rack.color = window.paint;
                    } else if (window.gridState === "erase") {
                        const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                        const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                        if (rack) {
                            const index = grid.racks.indexOf(rack);
                            if (index > -1) {
                                grid.racks.splice(index, 1);
                            }
                        }
                    } else {
                        // Place mode logic
                        if (mouseRack) {
                            const { x: gridX, y: gridY } = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                            if (gridX < 0 || gridX + mouseRack.width > grid.x || gridY < 0 || gridY + mouseRack.height > grid.y) return;
                            mouseRack.x = gridX * grid.size;
                            mouseRack.y = gridY * grid.size;
                        } else if (window.draggedRack) {
                            const { width, height } = window.draggedRack;
                            const { x: gridX, y: gridY } = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                            if (gridX + width > grid.x || gridY + height > grid.y) return;
                            mouseRack = new Rack(sketch, gridX * grid.size, gridY * grid.size, width, height);
                        }
                    }
                };

                sketch.mouseReleased = () => {
                    if (!mouseRack) return;

                    grid.racks.push(mouseRack);
                    mouseRack = undefined;
                    window.draggedRack = undefined;
                };
            }, document.querySelector("div#grid-area"));
        }
    };
})();

// Initialize when Blazor component loads
floorsetGrid.init();