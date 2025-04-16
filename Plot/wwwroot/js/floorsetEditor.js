// // Floorset editor/fixture script - Andrew Kennedy 

// // Get reference to the grid area
// var container = document.getElementById("container");
// var isAsc = true;

// // Dimensions of items
// var width = 50;
// var height = 50;
// var rows = 48;
// var cols = 48;
// var snap = 50;

// // Set the styles of the container based on the given dimensions
// container.style.height = (height * rows) + "px";
// container.style.width = (width * cols) + "px";

// // Set the grid area with a grid layout, and fill it with divs
// container.setAttribute("style", "display: grid; grid-column-gap: 0px;grid-row-gap: 0px;");
// container.style.gridTemplateColumns = `repeat(${cols},${width}px)`;
// container.style.gridTemplateRows = `repeat(${rows},${height}px)`;

// for (var i = 0; i < rows * cols; i++) {
//     $("<div class=\"grid-cell\"></div>").prependTo(container);
// }

// //Tristan Calay - 3/27/25
// //Added ID tracking to created fixtures!
// //Added paint mode detection
// var fixture_IDs = []; //Tracking of fixture box div IDs
// var paintModeColor = "null|null|transparent";
// var paintModeEnabled = false;

// //The set paint color function sets the paint_mode_color, which will be used in the onmouseenter event of the fixture div.
// function setPaintColor(newColor) {
//     console.log("Paint color recieved: " + newColor)
//     paintModeColor = newColor;
// }

// //The set paint enabled function enables or disables the paint mode - the listener for onmouseenter will abort early if disabled.
// function setPaintEnabled(isEnabled) {
//     paintModeEnabled = isEnabled;
//     fixture_IDs.forEach((element) => {
//         if (isEnabled) {
//             Draggable.get('#' + element).disable();
//         }
//         else {
//             Draggable.get('#' + element).enable();
//         }

//     })
// }

// /*
//     The createDraggable function creates a draggable fixture to add to the grid.
// */
// function createDraggable(event, size, color) {
//     //Tristan Calay
//     //Create a new ID for the new fixture - last index in array + 1, or 0 if no fixtures exist.
//     //In the future, when checking DB, populate the fixture ID array!
//     var new_id = 0;
//     if (fixture_IDs.length === 0) {
//         fixture_IDs.push(new_id);
//     }
//     else {
//         new_id = fixture_IDs[fixture_IDs.length - 1] + 1;
//         fixture_IDs.push(new_id)
//     }

//     // Get references to the sidebar and create a soon-to-be draggable fixture
//     var sidebar = document.getElementById("FloorsetSlideOut");
//     var newBox = document.createElement("div");
//     newBox.id = new Date().getTime();

//     // Make the box a child of the container (grid)
//     container.appendChild(newBox);

//     // Style the box according to the dimensions above
//     newBox.className = "box";
//     newBox.style.position = "absolute";
//     var boxSize = size.split("x");
//     newBox.style.height = boxSize[0] * height + "px";
//     newBox.style.width = boxSize[1] * width + "px";

//     newBox.style.background = color;
//     newBox.style.border = "3px solid black";
//     newBox.style.borderRadius = "8px";
//     newBox.id = new_id;

//     //Tristan Calay -- 3/27/25
//     //Add in listeners for mouse over for category painting.
//     newBox.addEventListener("mouseenter", onPaintColor);
//     newBox.addEventListener("mousedown", onPaintColor);
//     function onPaintColor(event) {
//         //console.log("Fixture mouse over detected!!! ID: " + newBox.id);
//         if (!paintModeEnabled) {
//             return;
//         }
//         //Check if mouse buttons contains the number 1, representing primary button.
//         console.log("Buttons equals " + event.buttons)
//         if (event.buttons % 2 === 1) {
//             //Tristan Calay
//             //paintModeColor contains the overall category "Men's, Women's, Accessories", 
//             //The specific item "Pants, Shorts, etc..."
//             //And the color code, split by the character '|'.
//             var colorString = paintModeColor.split("|")[2];
//             newBox.style.background = colorString;
//         }
//         /*In the future, add in some way to call back to the database that fixture 
//         with ID "newBox.id" has been changed to a certain category.
//         We should probably store the actual category string when a category list item is clicked!
//         */
//     }



//     // Get container position relative to viewport
//     var containerRect = container.getBoundingClientRect();

//     // Set the snapping dimensions for the box as it is dragged
//     var mouseX = event.clientX;
//     var mouseY = event.clientY;
//     var snappedX = Math.round((mouseX - snap * 2) / snap) * snap;
//     var snappedY = Math.round((mouseY - snap * 2) / snap) * snap;

//     newBox.style.left = snappedX + "px";
//     newBox.style.top = snappedY + "px";

//     // Create the draggable box 
//     var draggable = Draggable.create(newBox, {
//         bounds: container,
//         onDrag: function () {

//             // Using TweenLite, snap the box to a given snap distance,
//             // and make the animation smooth
//             TweenLite.to(newBox, 0.5, {
//                 x: Math.round(this.endX / snap) * snap,
//                 y: Math.round(this.y / snap) * snap,
//                 ease: Back.easeOut.config(2)
//             });
//         },
//         onDragEnd: function () {
//             // If the box is over the sidebar when dropped, remove it from view,
//             // otherwise set the z-index low so it is under the sidebar.
//             var boxRect = newBox.getBoundingClientRect();
//             var sidebarRect = sidebar.getBoundingClientRect();
//             containerRect = container.getBoundingClientRect();
//             if (isOverElement(boxRect, sidebarRect)) {
//                 container.removeChild(newBox);
//             }
//             else if (isOverlapping(newBox, container)) {
//                 container.removeChild(newBox);
//                 displayAlert("Cannot place an element over an existing element.");
//             }
//             else {
//                 newBox.style.zIndex = 0;
//             }
//         }
//     });
//     // Wait for 10ms, then dispatch a mousedown event to simulate a drag on the box.
//     setTimeout(() => {
//         var evt = new MouseEvent("mousedown", {
//             bubbles: true,
//             cancelable: true,
//             clientX: mouseX,
//             clientY: mouseY
//         });
//         newBox.dispatchEvent(evt);
//     }, 10);
// }
// //this boolean controls painting so that once the user is done, it prevents "painting" from happening unless 
// //they make a new div from the sidebar
// var isPaintingEnabled = false;

// //this function is specific to the employee only area as it uses "painting" for the div it creates
// function createDraggableEmployee(event, size, color) {
//     isPaintingEnabled = true;
//     var sidebar = document.getElementById("FloorsetSlideOut");
//     var newBox = document.createElement("div");
//     newBox.id = new Date().getTime(); // Unique ID
//     container.appendChild(newBox);

//     newBox.className = "box";
//     newBox.style.position = "absolute";
//     var boxSize = size.split("x");
//     newBox.style.height = boxSize[0] * height + "px";
//     newBox.style.width = boxSize[1] * width + "px";
//     newBox.style.background = color;
//     newBox.style.border = "3px solid black";
//     newBox.style.borderRadius = "8px";

//     //returns the boundaries of the container, set to containerRect variable
//     var containerRect = container.getBoundingClientRect();

//     var mouseX = event.clientX;
//     var mouseY = event.clientY;
//     var snappedX = Math.round((mouseX - containerRect.left - snap * 2) / snap) * snap;
//     var snappedY = Math.round((mouseY - containerRect.top - snap * 2) / snap) * snap;

//     //this prevents the box from going outside the container boundaries
//     snappedX = Math.max(0, Math.min(snappedX, containerRect.width - newBox.offsetWidth));
//     snappedY = Math.max(0, Math.min(snappedY, containerRect.height - newBox.offsetHeight));

//     newBox.style.left = snappedX + "px";
//     newBox.style.top = snappedY + "px";
//     var draggable = Draggable.create(newBox, {
//         bounds: container,
//         onDrag: function () {
//             //snap to grid while adding
//             TweenLite.to(newBox, 0.5, {
//                 x: Math.round(this.endX / snap) * snap,
//                 y: Math.round(this.y / snap) * snap,
//                 ease: Back.easeOut.config(2)
//             });
//         },
//         onDragEnd: function () {
//             var boxRect = newBox.getBoundingClientRect();
//             var sidebarRect = sidebar.getBoundingClientRect();
//             containerRect = container.getBoundingClientRect();

//             //if there is an overlap with the sidebar or the containers children then the element will be removed
//             if (isOverElement(boxRect, sidebarRect)) {
//                 container.removeChild(newBox);
//             } else if (isOverlapping(newBox, container)) {
//                 container.removeChild(newBox);
//                 displayAlert("Cannot place an element over an existing element.");
//             } else {
//                 newBox.style.zIndex = 0;
//             }
//         }
//     });
//     //if there is an overlap then removes the box
//     if (isOverlapping(newBox, container)) {
//         container.removeChild(newBox);
//     }

//     setTimeout(() => {
//         var evt = new MouseEvent("mousedown", {
//             bubbles: true,
//             cancelable: true,
//             clientX: mouseX,
//             clientY: mouseY
//         });
//         newBox.dispatchEvent(evt);
//     }, 10);
// }

// //this method paints the boxes and handles adding and creating new elements
// function paintEmployeeBoxes(event, size, color) {
//     var newBox = document.createElement("div");
//     newBox.id = new Date().getTime(); // Unique ID based on timestamp

//     container.appendChild(newBox);

//     newBox.className = "box";
//     newBox.style.position = "absolute";
//     var boxSize = size.split("x");
//     newBox.style.height = boxSize[0] * height + "px";
//     newBox.style.width = boxSize[1] * width + "px";
//     newBox.style.background = color;
//     newBox.style.border = "3px solid black";
//     newBox.style.borderRadius = "8px";

//     var containerRect = container.getBoundingClientRect();

//     var mouseX = event.clientX;
//     var mouseY = event.clientY;

//     var snappedX = Math.round((mouseX - containerRect.left - snap * 2) / snap) * snap;
//     var snappedY = Math.round((mouseY - containerRect.top - snap * 2) / snap) * snap;

//     //this prevents the box from going outside the container boundaries
//     snappedX = Math.max(0, Math.min(snappedX, containerRect.width - newBox.offsetWidth));
//     snappedY = Math.max(0, Math.min(snappedY, containerRect.height - newBox.offsetHeight));

//     newBox.style.left = snappedX + "px";
//     newBox.style.top = snappedY + "px";
//     var draggable = Draggable.create(newBox, {
//         bounds: container,
//         onDrag: function () {
//             //snap to grid while adding
//             TweenLite.to(newBox, 0.5, {
//                 x: Math.round(this.endX / snap) * snap,
//                 y: Math.round(this.y / snap) * snap,
//                 ease: Back.easeOut.config(2)
//             });
//         },
//         onDragEnd: function () {
//             var boxRect = newBox.getBoundingClientRect();
//             var sidebarRect = sidebar.getBoundingClientRect();
//             containerRect = container.getBoundingClientRect();

//             //if there is an overlap with the sidebar or the containers children then the element will be removed
//             if (isOverElement(boxRect, sidebarRect)) {
//                 container.removeChild(newBox);
//             }
//             // Tristan Calay 4/1/2025 - Commenting out alerts for isoverlapping.
//             // else if (isOverlapping(newBox, container)) {
//             //     container.removeChild(newBox);
//             //     displayAlert("Cannot place an element over an existing element.");
//             // } 
//             else {
//                 newBox.style.zIndex = 0;
//             }
//         }
//     });
//     //Temporarily comment this collision detection out.
//     //if there is an overlap then removes the box
//     // if (isOverlapping(newBox, container)) {
//     //     container.removeChild(newBox);
//     // }

//     setTimeout(() => {
//         var evt = new MouseEvent("mousedown", {
//             bubbles: true,
//             cancelable: true,
//             clientX: mouseX,
//             clientY: mouseY
//         });
//         newBox.dispatchEvent(evt);
//     }, 10);

//     //this method paints the boxes and handles adding and creating new elements
//     function paintEmployeeBoxes(event, size, color) {
//         var newBox = document.createElement("div");
//         newBox.id = new Date().getTime(); // Unique ID based on timestamp
//         container.appendChild(newBox);

//         newBox.className = "box";
//         newBox.style.position = "absolute";
//         var boxSize = size.split("x");
//         newBox.style.height = boxSize[0] * height + "px";
//         newBox.style.width = boxSize[1] * width + "px";
//         newBox.style.background = color;
//         newBox.style.border = "3px solid black";
//         newBox.style.borderRadius = "8px";

//         var mouseX = event.clientX;
//         var mouseY = event.clientY;

//         var snappedX = Math.round((mouseX - containerRect.left - snap * 2) / snap) * snap;
//         var snappedY = Math.round((mouseY - containerRect.top - snap * 2) / snap) * snap;

//         //this prevents the box from going outside the container boundaries
//         snappedX = Math.max(0, Math.min(snappedX, containerRect.width - newBox.offsetWidth));
//         snappedY = Math.max(0, Math.min(snappedY, containerRect.height - newBox.offsetHeight));

//         newBox.style.left = snappedX + "px";
//         newBox.style.top = snappedY + "px";
//         var draggable = Draggable.create(newBox, {
//             bounds: container,
//             onDrag: function () {
//                 //this snaps the box to a given distnace
//                 TweenLite.to(newBox, 0.5, {
//                     x: Math.round(this.endX / snap) * snap,
//                     y: Math.round(this.y / snap) * snap,
//                     ease: Back.easeOut.config(2)
//                 });
//             },
//             onDragEnd: function () {
//                 var boxRect = newBox.getBoundingClientRect();
//                 var sidebarRect = sidebar.getBoundingClientRect();
//                 containerRect = container.getBoundingClientRect();
//                 if (isOverElement(boxRect, sidebarRect)) {
//                     container.removeChild(newBox);
//                 } else if (isOverlapping(newBox, container)) {
//                     container.removeChild(newBox);
//                     displayAlert("Cannot place an element over an existing element.");
//                 } else {
//                     newBox.style.zIndex = 0;
//                 }
//             }
//         });

//         //if there is an overlap then it removes the box
//         if (isOverlapping(newBox, container)) {
//             container.removeChild(newBox);
//         }
//     }

//     //this event listener handles the click event
//     document.addEventListener("mousedown", function (event) {
//         if (isPaintingEnabled) {
//             paintEmployeeBoxes(event, size, color);
//             document.addEventListener("mousemove", function (event) {
//                 if (isPaintingEnabled) {
//                     paintEmployeeBoxes(event, size, color);
//                 }
//             });
//         }
//     });

//     //on the mouse up, set the flag to false to stop painting
//     document.addEventListener("mouseup", function () {
//         isPaintingEnabled = false;
//     });
// }

// //this method checks whether or not there is an overlap between the passed element and any child element in the container
// function isOverlapping(draggableRect, containerElement) {
//     var overlapFound = false;
//     var children = containerElement.children;
//     var draggableArea = draggableRect.getBoundingClientRect();

//     Array.from(children).forEach(function (child) {
//         if (child.className === "box" && child !== draggableRect) {
//             var area = child.getBoundingClientRect();
//             if (draggableArea.top < area.bottom && draggableArea.bottom > area.top &&
//                 draggableArea.left < area.right && draggableArea.right > area.left) {
//                 overlapFound = true;
//             }
//         }
//     });
//     return overlapFound;
// }


// //this function is to check if the box is over the sidebar
// function isOverElement(boxRect, sidebarRect) {
//     return (
//         boxRect.top < sidebarRect.bottom &&
//         boxRect.bottom > sidebarRect.top &&
//         boxRect.left < sidebarRect.right &&
//         boxRect.right > sidebarRect.left
//     );
// }



/*
    The flipOrder function changes the flex column direction when the button is selected.
*/
function flipOrder() {
    // Change the flex direction based on the boolean value           
    if (isAsc) {
        document.querySelector(".fixture-area").style.flexDirection = "column-reverse";
    } else {
        document.querySelector(".fixture-area").style.flexDirection = "column";
    }
    // Flip the isAsc value to indicate a reversed column
    isAsc = !isAsc;
}

/*
    This function grabs the current canvas image from an open floorset and returns it
*/

function getCanvasImage(callback) {
    const canvas = document.querySelector('canvas');
    if (!canvas) {
        console.error("Canvas not found!");
        return null;
    }

    //save current zoom
    const p5 = window.p5Instance;
    const grid = window.gridInstance;

    const originalScale = grid.scale;
    const originalWidth = p5.width;
    const originalHeight = p5.height;



    //set scale and center the grid manually
    grid.scale = 1;
    const fullWidth = grid.width * grid.size;
    const fullHeight = grid.height * grid.size;
    p5.resizeCanvas(fullWidth, fullHeight);
    grid.resize();

    //freeze drawing
    p5.noLoop();
    //force draw with requestAnimationFrame to get the full canvas size
    requestAnimationFrame(() => {
        p5.redraw();
        const image = canvas.toDataURL("image/png");

        //Restore everything
        p5.resizeCanvas(originalWidth, originalHeight);
        grid.scale = originalScale;
        grid.resize();
        p5.redraw();
        p5.loop();

        callback(image);
    });
}

/*
    The downloadCanvasImage function grabs the main canvas and downloads its image 
    to the user's device when the Print button is clicked
*/
function downloadCanvasImage(floorsetName) {
    getCanvasImage((image) => {
        if (!image) {
            console.error("No image data available.");
            return;
        }

        const link = document.createElement("a");
        link.href = image;
        const name = floorsetName.replace(/[^a-z0-9_\-]/gi, "_").toLowerCase();
        link.download = `${name}_Layout.png`;

        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });
}
/*
    The addFixtureClose function adds an event listener to the add button in the Add Fixture modal.
*/
function addFixtureClose(dotNet) {
    $("#addFixture").on('hidden.bs.modal', function (e) {
        // Call the C# function to clear the data from the Add Fixture modal
        dotNet.invokeMethodAsync("ClearTempFixtureData");

        // Clear the data from the image input
        var imgInput = document.querySelector('#addFixture .img-input .ImageInput');
        imgInput.style.backgroundImage = "";
        imgInput.classList.add("dashed-border");

        // Remove the modal-backdrop on close
        document.querySelectorAll('.modal-backdrop')?.forEach(m => m.remove());
    })
}

function isOverScrollable() {
    // Is the side bar over a scroll area
    // (Andrew Miller - 4/6/2025)

    // Checks if the user is hovering over a scrollable component.

    let scrollables = document.querySelectorAll('.scrollable');

    for (let scrollable of scrollables) {
        if (scrollable.matches(':hover')) { return true; }
    }

    return false;
}

/*
    The searchInputChange function adds an event listener to the fixture search bar.
*/
function searchInputChange() {
    setTimeout(() => {
        document.getElementById('search').addEventListener("keyup", function (e) {
            // Get the text from the search field
            var searchText = this.value;

            // Loop through each fixture tile 
            Array.from(document.getElementsByClassName('fixture')).forEach(function (fixture) {
                // Get the name of the fixture
                var nameElement = fixture.firstChild.getElementsByTagName('p')[0];

                // If any fixture's name contains the search string, display the tile
                if (nameElement && nameElement.innerText.toLowerCase()
                    .indexOf(searchText.toLowerCase()) > -1) {
                    fixture.style.display = 'flex';
                } else {
                    // Otherwise, do not display the tile
                    fixture.style.display = 'none';
                }
            })
        })
    }, 500);
}

//method to set the background image of the edit modal imageinput to the fixture's image
function SetBackgroundImage(elementId, strUrl) {
    const element = document.getElementById(elementId);
    console.log("here");
    if (element) {
        console.log(strUrl.includes("url"));
        element.style.backgroundImage = "url(" + strUrl + ")";
        element.style.backgroundSize = "cover";
        element.style.backgroundPosition = "center";
        element.classList.remove("dashed-border");
    }
}

//function used to clear the imageInput background
function ClearBackgroundImage(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.backgroundImage = "none";
        element.classList.add("dashed-border");
    }
}

// function to get the URL of the image from the elemebts style tag
function getBackgroundImageUrl(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        const style = window.getComputedStyle(element);
        const backgroundImage = style.backgroundImage;
        // Extract the URL from the backgroundImage string in the style tag
        const urlMatch = backgroundImage.match(/url\(["']?(.*?)["']?\)/);
        //return urlMatch[1], which is the data URL string
        return urlMatch && urlMatch[1] ? urlMatch[1] : null;
    }
    return null;
}

/*
    The toggleModal function toggles the visibility of a given modal.
*/
function toggleModal(modalId, showModal) {
    // If the modal exists
    if (document.getElementById(modalId)) {
        let modal = bootstrap.Modal.getOrCreateInstance(`#${modalId}`);
        // Show the modal when showModal is true
        if (showModal) {
            modal.show();
        }
        else {
            if (document.getElementById(modalId) != undefined) {
                // Otherwise, remove all residuals of the modal
                document.body.style.removeProperty("overflow");
                document.body.style.removeProperty("padding-right");
                document.body.classList.remove("modal-open");
                document.getElementById(modalId).classList.remove('show');
                document.getElementById(modalId).style.display = "none";
                document.querySelectorAll('.modal-backdrop')?.forEach(m => m.remove());
            }
        }
    }
}

/*
    The imageEventListener function creates an event listener for the image inputs on the modals.
*/
function imageEventListener(dotNet) {
    document.querySelectorAll('.img-input').forEach(i => i.firstChild.addEventListener('change', (e) => {
        console.log(e.target);
        dotNet.invokeMethodAsync("UpdateImage", e.target.value);
    }));
}

/*
    The alertUser function sends an alert to the user with a given message.
*/
function displayAlert(msg) {
    alert(msg);
}

/* END FLOORSET SCRIPTS */


/* Luke Wollenweber - 3/22/2025
   js function to re-add cells based on new Dimensions 
   updated 3/27 by Luke Wollenweber */
function UpdateGridDimensions(passedLength, passedWidth) {
    window.gridHeight = passedLength;
    window.gridWidth = passedWidth;
}

/*  Andrew Kennedy - 4/15/2025
    The openSaveToast method opens a bootstrap toast.
*/
function openSaveToast(toastID) {
    var saveToast = bootstrap.Toast.getOrCreateInstance(document.getElementById(toastID));
    saveToast.show();
}

// function createDraggable(event) {
//     const width = Number(event.target.getAttribute("data-width")),
//         height = Number(event.target.getAttribute("data-height")),
//         name = String(event.target.getAttribute("data-value")),
//         fixtureTuid = Number(event.target.getAttribute("data-fixture-tuid"));
//     window.draggedFixture = { width, height, name };
// }