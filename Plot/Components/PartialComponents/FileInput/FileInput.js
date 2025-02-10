window.initializeDragDrop = (dotnetHelper) => {
    const dropZone = document.getElementById("dropZone");

    if (!dropZone) return;

    dropZone.addEventListener("dragover", (event) => {
        event.preventDefault(); // Prevent default to allow drop
        dropZone.classList.add("drag-over");
    });

    dropZone.addEventListener("dragleave", () => {
        dropZone.classList.remove("drag-over");
    });

    dropZone.addEventListener("drop", async (event) => {
        event.preventDefault();
        dropZone.classList.remove("drag-over");

        let files = event.dataTransfer.files;
        if (files.length > 0) {
            await dotnetHelper.invokeMethodAsync("DropFile");
        }
    });
};

window.getDroppedFiles = async function () {
    return document.getElementById("dropZone").files;
};
