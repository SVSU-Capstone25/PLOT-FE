let gridInstance;      // will hold our Grid
let p5Instance;        // will hold our p5

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
    const racks = this.racks;
    console.log("Called delete rack with ID: " + id)
    console.log("Racks: " + racks)
    for (var i = 0; i < racks.length; i++) {
        console.log("Checking index " + i)
        const rack = this.racks[i]
        if (rack.id === id) {
            console.log("Rack deleted: " + id)
            this.racks.splice(i, 1);
            return;
        }
    }
}


class Rack {
    constructor(p5, x, y, width, height, id, isEmployee = false) {
        this.p5 = p5;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.color = this.p5.color(255, 255, 255);
        this.id = id;
        this.isEmployee = isEmployee;
        if (isEmployee) {
            this.color = this.p5.color(255, 0, 0, 100);
        }
        else {
            this.color = this.p5.color(255, 255, 255);
        }
    }

    draw(gridSize) {
        this.p5.fill(this.color);
        this.p5.stroke(0);
        this.p5.strokeWeight(3);
        this.p5.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
        this.p5.push();
        this.p5.fill(this.color);
        this.p5.stroke(0);
        if (!this.isEmployee) {
            this.p5.strokeWeight(3);
        }
        this.p5.rect(this.x, this.y, this.width * gridSize, this.height * gridSize);
        this.p5.pop();
    }
}

class Grid {

    constructor(p5) {
        this.p5 = p5;
        this.size = 30;
        this.scale = 1;
        this.racks = [];
        this.width = 1;
        this.height = 1;
        this.resize();
    }

    addFixtureInstanceToGrid(id, x, y, width, length) {
        this.racks.push(new Rack(this.p5, x, y, width, length, id, false));
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

        for (const rack of this.racks) {
            rack.draw(this.size);
        }
    }
}

function sketch(p5) {
    let mouseRack;
    p5.setup = () => {
        p5Instance = p5;
        gridInstance = new Grid(p5);

        window.p5Instance = p5Instance;
        window.gridInstance = gridInstance;

        window.gridState = "place";

        p5.createCanvas(p5.windowWidth, p5.windowHeight);
        p5.frameRate(30);

        document.oncontextmenu = function () {
            const coords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY)
            //console.log("Determine if should display context menu: " + coords.x + ", " + coords.y)
            if (gridInstance.isOnGrid(coords.x, coords.y)) {
                //console.log("Should not display context menu: Is on grid!")
                return false;
            }
        }

        window.paint = '#fff';
    }


    p5.draw = () => {
        p5.background(220);
        p5.push();
        gridInstance.draw();
        mouseRack?.draw(gridInstance.size);
        p5.pop();
    };

    p5.windowResized = () => {
        p5.resizeCanvas(p5.windowWidth, p5.windowHeight);
        gridInstance.resize();
    }

    p5.mouseWheel = (event) => {
        if (event.delta > 0) {
            gridInstance.scale += 0.1;
        } else {
            gridInstance.scale -= 0.1;
        }

        gridInstance.scale = Math.max(0.1, gridInstance.scale);
        gridInstance.resize();
    }

    p5.mousePressed = () => {
        if (window.gridState === "place") {
            const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
            const rack = gridInstance.getRackAt(gridCoords.x, gridCoords.y);
            if (rack) {
                const index = gridInstance.racks.indexOf(rack);
                if (index > -1) {
                    gridInstance.racks.splice(index, 1);
                    mouseRack = rack;
                }
            }
        } else if (window.gridState === "erase") {
            const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
            const rack = gridInstance.getRackAt(gridCoords.x, gridCoords.y);
            if (rack) {
                const index = gridInstance.racks.indexOf(rack);
                if (index > -1) {
                    gridInstance.racks.splice(index, 1);
                }
            }
        } else {
            const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
            const rack = gridInstance.getRackAt(gridCoords.x, gridCoords.y);
            if (rack) rack.color = window.paint;
        }
    };

    p5.mouseDragged = () => {
        if (window.gridState === "paint") {
            const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
            const rack = gridInstance.getRackAt(gridCoords.x, gridCoords.y);
            if (rack) rack.color = window.paint;
        } else if (window.gridState === "erase") {
            const gridCoords = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
            const rack = gridInstance.getRackAt(gridCoords.x, gridCoords.y);
            if (rack) {
                const index = gridInstance.racks.indexOf(rack);
                if (index > -1) {
                    gridInstance.racks.splice(index, 1);
                }
            }
        } else {
            // Place mode logic
            if (mouseRack) {
                const { x: gridX, y: gridY } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
                if (gridX < 0 || gridX + mouseRack.width > gridInstance.x || gridY < 0 || gridY + mouseRack.height > gridInstance.y) return;
                mouseRack.x = gridX * gridInstance.size;
                mouseRack.y = gridY * gridInstance.size;
            } else if (window.draggedRack) {
                const { width, height } = window.draggedRack;
                const { x: gridX, y: gridY } = gridInstance.toGridCoordinates(p5.mouseX, p5.mouseY);
                if (gridX + width > gridInstance.x || gridY + height > gridInstance.y) return;
                mouseRack = new Rack(p5, gridX * gridInstance.size, gridY * gridInstance.size, width, height);
            }
        }
    }

    p5.mouseReleased = () => {
        if (!mouseRack) return;

        gridInstance.racks.push(mouseRack);
        mouseRack = undefined;
        window.draggedRack = undefined;
    }
};


//Test function to save the canvas as an image
document.addEventListener('keypress', event => {
    if (event.keyCode == 83) { // "S" Key
        p5Instance.saveCanvas('floorsetGrid', 'jpg');
        console.log("Saving Image!");
    }
})

function addFixtureOnLoad(id, x, y, width, length, color) {
    setTimeout(function () {
        let newRack = new Rack(p5Instance, x, y, width, length, id);
        newRack.color = color;
        this.racks.push(newRack);
    }, 500);
}

window.initP5 = (elementId) => {
    new p5(sketch, elementId);
};

window.addFixture = (id, x, y, width, length) => {
    if (!window.gridInstance) {
        console.error("Grid not initialized yet!");
        return;
    }

    window.gridInstance.addFixtureInstanceToGrid(id, x, y, width, length);
};

window.updateStoreSize = (width, height) => {
    if (!window.gridInstance) {
        console.error("Grid not initialized yet!");
        return;
    }

    window.gridInstance.width = width;
    window.gridInstance.height = height;
    window.gridInstance.resize();
}

function createDraggable(event) {
    const width = Number(event.target.getAttribute("data-width"));
    const height = Number(event.target.getAttribute("data-height"));
    const name = String(event.target.getAttribute("data-value"));
    window.draggedRack = { width, height, name };
}