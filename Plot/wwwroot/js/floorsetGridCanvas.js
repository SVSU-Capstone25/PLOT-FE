//floorsetGridCanvas.js

function setPaintMode(enabled) {
    console.log("Setting paint mode " + enabled)
    //Translate true/false to "paint" / "place"
    if (enabled) {
        window.gridState = 'paint'
    }
    else {
        window.gridState = 'place'
    }
    console.log("window gridstate now: " + window.gridState);
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

// Tristan Calay 4/2/25 - Toggles for employee paint/erase mode.
var isEmployeePaintEnabled = false;
var isEmployeeEraseEnabled = false;

function setEmployeePaint(newPaint) {
    isEmployeePaintEnabled = newPaint;
    console.log("Employee paint mode is " + isEmployeePaintEnabled);
    //var marker = document.getElementById("employeePaintEnabledMarker");
    if (isEmployeePaintEnabled) {
        console.log("Setting marker green...")
        //marker.style.color = "green";
        window.gridState = 'employeeMode';
    }
    else {
        console.log("Setting marker black...")
        //marker.style.color = "black";
        window.gridState = 'place';
    }
}

function setEmployeeErase(newErase) {
    isEmployeeEraseEnabled = newErase;
    if (isEmployeeEraseEnabled) {
        window.gridState = 'employeeMode';
    }
}

//Tristan Calay 4/7/25
//Remove a rack from the racks array by ID
function deleteRackByID(id) {
    const racks = grid.racks;
    console.log("Called delete rack with ID: " + id)
    console.log("Racks: " + racks)
    for (var i = 0; i < racks.length; i++) {
        console.log("Checking index " + i)
        const rack = grid.racks[i]
        if (rack.id === id) {
            console.log("Rack deleted: " + id)
            grid.racks.splice(i, 1);
            return;
        }
    }
}
class Rack {
    constructor(sketch, x, y, width, height, id, isEmployee = false) {
        this.sketch = sketch;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.color = this.sketch.color(255, 255, 255);
        this.id = id;
        this.isEmployee = isEmployee;
        if (isEmployee) {
            this.color = this.sketch.color(255, 0, 0, 100);
        }
        else {
            this.color = this.sketch.color(255, 255, 255);
        }
    }

    draw(gridSize) {
        this.sketch.fill(this.color);
        this.sketch.stroke(0);
        this.sketch.strokeWeight(3);
        this.sketch.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
        this.sketch.push();
        this.sketch.fill(this.color);
        this.sketch.stroke(0);
        if (!this.isEmployee) {
            this.sketch.strokeWeight(3);
        }
        this.sketch.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
        this.sketch.pop();
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

    isOnGrid(gridX, gridY) {
        if (gridX >= 0 && gridX < this.x && gridY >= 0 && gridY < this.y) {
            return true;
        }
        return false;
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

var grid, mouseRack;

const floorsetGrid = (function () {
    let sketchInstance = null;

    // Flag to make sure rack creation is called only once
    let rackCreated = undefined;

    return {
        init() {
            sketchInstance = new p5((sketch) => {
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

                    document.oncontextmenu = function () {
                        const coords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY)
                        //console.log("Determine if should display context menu: " + coords.x + ", " + coords.y)
                        if (grid.isOnGrid(coords.x, coords.y)) {
                            //console.log("Should not display context menu: Is on grid!")
                            return false;
                        }
                    }

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
                                            break;
                                        case "employeeMode":
                                            //Handle drawing employee boxes on the grid.
                                            if (isEmployeeEraseEnabled && rack.isEmployee) {
                                                //Erase this rack if it is an employee area
                                                //console.log("I should erase this rack")
                                                const index = grid.racks.indexOf(rack);
                                                if (index > -1) {
                                                    grid.racks.splice(index, 1);
                                                }
                                            }
                                            //Do nothing if an employee area already exists under the cursor.
                                            //console.log("Don't place a new rack here, one exists already.")
                                            break;
                                        case "erase":
                                            grid.racks.splice(index, 1);
                                            break;
                                        case "paint":
                                            if (!rack.isEmployee) {
                                                rack.color = window.paint;
                                                paintFixtureByID(rack.id);
                                            }
                                            break;
                                    }
                                    break;

                                case 2:
                                    //Context Window Mode
                                    //console.log("Display context window of rack.")
                                    selectFixtureByID(rack.id);
                            }
                        }
                    }
                    else {
                        if (window.gridState === "employeeMode") {
                            if (isEmployeeEraseEnabled || !grid.isOnGrid(gridCoords.x, gridCoords.y)) {
                                //Don't place a rack if erase mode is on, or is off grid
                                return;
                            }
                            //console.log("I should put a rack here!")
                            let newEmployeeRack = new Rack(sketch, gridCoords.x * grid.size, gridCoords.y * grid.size, 1, 1, true);
                            grid.racks.push(newEmployeeRack);
                        }
                    }
                };

                sketch.mouseDragged = () => {
                    const gridCoords = grid.toGridCoordinates(sketch.mouseX, sketch.mouseY);
                    const rack = grid.getRackAt(gridCoords.x, gridCoords.y);
                    switch (window.gridState) {
                        case "paint":
                            if (rack && !rack.isEmployee) {
                                rack.color = window.paint;
                                paintFixtureByID(rack.id);
                            }
                            break;
                        case "erase":
                            if (rack && !rack.isEmployee) {
                                const index = grid.racks.indexOf(rack);
                                if (index > -1) {
                                    grid.racks.splice(index, 1);
                                }
                            }
                            break;
                        case "employeeMode":
                            //Handle drawing employee boxes on the grid.
                            if (rack) {
                                if (isEmployeeEraseEnabled && rack.isEmployee) {
                                    //Erase this rack if it is an employee area
                                    //console.log("I should erase this rack")
                                    const index = grid.racks.indexOf(rack);
                                    if (index > -1) {
                                        grid.racks.splice(index, 1);
                                    }
                                }
                                //console.log("Don't place a new rack here, one exists already.")
                                //Do nothing if an employee area already exists under the cursor.
                            }
                            else {
                                if (isEmployeeEraseEnabled || !grid.isOnGrid(gridCoords.x, gridCoords.y)) {
                                    //Don't place a rack if erase mode is on.
                                    return;
                                }
                                //console.log("I should put a rack here!")
                                //TODO: Hook this into fixtures C# side for employee area fixture!!!!!!!
                                let newEmployeeRack = new Rack(sketch, gridCoords.x * grid.size, gridCoords.y * grid.size, 1, 1, -1, true);
                                grid.racks.push(newEmployeeRack);

                            }
                        case "place":
                            // Place mode logic
                            if (mouseRack) {
                                if (gridCoords.x < 0 || gridCoords.x + mouseRack.width > grid.x || gridCoords.y < 0 || gridCoords.y + mouseRack.height > grid.y) return;

                                mouseRack.x = gridCoords.x * grid.size;
                                mouseRack.y = gridCoords.y * grid.size;
                            } else if (window.draggedRack) {
                                const { width, height, name } = window.draggedRack;
                                if (gridCoords.x + width > grid.x || gridCoords.y + height > grid.y) return;
                                mouseRack = new Rack(sketch, gridCoords.x * grid.size, gridCoords.y * grid.size,
                                    width, height, -1);
                                rackCreated = name;

                            }
                            break;
                    }
                }

                sketch.mouseReleased = () => {
                    if (!mouseRack) return;
                    if (rackCreated) {
                        addedRack = mouseRack;
                        jsCreateNewFixture(rackCreated).then(id => {
                            addedRack.id = id
                            rackCreated = undefined;
                        })
                    }
                    grid.racks.push(mouseRack);
                    mouseRack = undefined;
                    window.draggedRack = undefined;

                };
            }, document.querySelector("div#grid-area"));
        }
    };
})();

// Initialize when Blazor component loads
//floorsetGrid.init();