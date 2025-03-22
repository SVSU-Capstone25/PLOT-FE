/* File Purpose: Global JS file for ImageInput custom component
   which serves the purpose of enabling image uploads.

   Author: Andrew Miller (3/22/2025)
   File already written but no previous author given. 
   
   Added check for empty file to OnImageSelected function.
   This prevents a false "Please upload an image file"
   alert if you click on "Cancel" in File Explorer. 
   
   Added line to make display of upload image header none
   after uploading image so the label doesn't go over the image.
   
   Added event listeners for dragover and drop to document
   to prevent image url from opening in new tab.

*/
window.initializeImageInput = (id) => {
    // get the drag/drop area as well as the file input
    const dropArea = document.getElementById(`${id}`);
    const fileInput = document.getElementById(`${id}-fileInput`);

    // add the event listener for when you click on the 
    // drag/drop area that opens the file input
    dropArea.addEventListener("click", () => fileInput.click());

    // add effect when something is dragged over the drag/drop area
    dropArea.addEventListener("dragover", (event) => {
        event.preventDefault();
        dropArea.classList.add("drag-over");
        event.dataTransfer.dropEffect = "copy";
    });

    // remove effect when something is dragged away from the drag/drop area
    dropArea.addEventListener("dragleave", () => {
        dropArea.classList.remove("drag-over");
    });

    // when a file is dropped in the drag/drop area, remove effect
    // put the file in the file input
    dropArea.addEventListener("drop", (event) => {
        event.preventDefault();
        dropArea.classList.remove("drag-over");

        if (event.dataTransfer.files.length > 0) {
            fileInput.files = event.dataTransfer.files;
            fileInput.dispatchEvent(new Event("change"));
        }
    });

    // Prevent the default behavior when an image is dropped anywhere on the document
    // Stops image URLs from opening in new tabs when the image lands outside the drop
    // area
    document.addEventListener("dragover", (event) => {
        event.preventDefault();
    });

    document.addEventListener("drop", (event) => {
        event.preventDefault();
    });
    
}

// if a valid image file was selected, change the background 
// of the drag/drop area to serve as a preview
function onImageSelected(event, id) {
    const file = event.target.files[0];

    // Represents image header. Keep argument in agreement with
    // the same from the ImageInput custom component
    let uploadImageHeader = document.getElementById(`${id}-header`);

    if(!file){  // cancel button in file explorer returns an empty file object
        return;
    }

    if (file && file.type.startsWith("image/")) {
        const reader = new FileReader();
        reader.onload = function (e) {
        let dropArea = document.getElementById(`${id}`);

        dropArea.style.backgroundImage = `url(${e.target.result})`;
        dropArea.style.backgroundSize = "cover";
        dropArea.style.backgroundPosition = "center";
        dropArea.classList.remove("dashed-border");

        // Place upload image header behind image
        uploadImageHeader.style.display = "none";
        };
        
        reader.readAsDataURL(file);
    } else {
        alert("Please upload a valid image file.");
    }
}