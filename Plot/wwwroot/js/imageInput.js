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
}

// if a valid image file was selected, change the background 
// of the drag/drop area to serve as a preview
function onImageSelected(event, id) {
    const file = event.target.files[0];

    if (file && file.type.startsWith("image/")) {
        const reader = new FileReader();
        reader.onload = function (e) {
            let dropArea = document.getElementById(`${id}`);

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