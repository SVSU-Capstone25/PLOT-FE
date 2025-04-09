/* File Purpose: Global JS file for ImageInput custom component
   which serves the purpose of enabling image uploads.

   Author: Andrew Miller (3/22/2025)
   File already written but no previous author given. 
   
   Added check for empty file to OnImageSelected function.
   This prevents a false "Please upload an image file"
   alert if you click on "Cancel" in File Explorer. 
   
*/
window.initializeImageInput = (id) => {
    // get the drag/drop area as well as the file input
    const dropArea = document.getElementById(`${id}-container`);
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

}

// if a valid image file was selected, change the background 
// of the drag/drop area to serve as a preview
function onImageSelected(id) {
    console.log("id is " + id);
    const fileInput = document.getElementById(`${id}-fileInput`);
    const file = fileInput.files[0];

    if (!file) {  // cancel button in file explorer returns an empty file object
        return;
    }

    if (file && file.type.startsWith("image/")) {
        const reader = new FileReader();
        reader.onload = function (e) {
            let dropArea = document.getElementById(`${id}-container`);

            dropArea.style.backgroundImage = `url(${e.target.result})`;
            dropArea.style.backgroundSize = "cover";
            dropArea.style.backgroundPosition = "center";
            dropArea.classList.remove("dashed-border");

        };

        reader.readAsDataURL(file);
    } else {
        alert("Please upload a valid image file.");
    }
}