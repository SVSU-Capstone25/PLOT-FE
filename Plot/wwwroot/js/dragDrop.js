// Orignally done by Andrew Kennedy 
window.dragDrop = function(className, sidebarSelector, gridSelector, gridSize = 50) {
    let selectedElement = null;

    interact(className).draggable({
        autoScroll: true,
        listeners: {
            start(event) {
                const target = event.target;
                const isFromSidebar = target.closest(sidebarSelector) !== null;

                if (isFromSidebar) {
                    // Clone the element from the sidebar
                    const clonedElement = target.cloneNode(true);
                    clonedElement.id = "dragged-" + Date.now();

                    // Apply styles for positioning
                    clonedElement.style.position = "absolute";
                    clonedElement.style.zIndex = "1000";
                    clonedElement.style.opacity = "1";

                    // Append the clone to the body
                    document.body.appendChild(clonedElement);

                    // Get bounding box of original element
                    const rect = target.getBoundingClientRect();
                    const offsetX = event.pageX - (rect.left + window.scrollX);
                    const offsetY = event.pageY - (rect.top + window.scrollY);

                    // Set absolute position for clone
                    clonedElement.style.left = `${event.pageX - offsetX}px`;
                    clonedElement.style.top = `${event.pageY - offsetY}px`;

                    clonedElement.dataset.x = clonedElement.offsetLeft;
                    clonedElement.dataset.y = clonedElement.offsetTop;

                    event.target.dataset.clonedId = clonedElement.id;
                    selectedElement = clonedElement;
                } else {
                    selectedElement = target;
                }
            },
            move(event) {
                let target = event.target;
                const clonedElement = document.getElementById(target.dataset.clonedId);
                if (clonedElement) {
                    target = clonedElement;
                }

                let x = (parseFloat(target.dataset.x) || 0) + event.dx;
                let y = (parseFloat(target.dataset.y) || 0) + event.dy;

                target.style.left = `${x}px`;
                target.style.top = `${y}px`;

                target.dataset.x = x;
                target.dataset.y = y;

                selectedElement = target;
            },
            end(event) {
                let target = event.target;
                let clonedElement = document.getElementById(target.dataset.clonedId);
                const sidebar = document.querySelector(sidebarSelector);
                const grid = document.querySelector(gridSelector);

                if (clonedElement) {
                    const clonedRect = clonedElement.getBoundingClientRect();
                    const sidebarRect = sidebar.getBoundingClientRect();

                    if (isOverElement(clonedRect, sidebarRect)) {
                        clonedElement.remove();
                        return;
                    } else {
                        clonedElement.style.zIndex = "10";
                        clonedElement.dataset.clonedId = "";
                        selectedElement = clonedElement;
                    }
                }

                // **🟢 Get Grid Position for Snapping**
                const gridRect = grid.getBoundingClientRect();

                // Convert absolute position to grid-relative position
                let relativeX = parseFloat(target.style.left) - gridRect.left;
                let relativeY = parseFloat(target.style.top) - gridRect.top;

                // **Snap to the nearest grid square**
                let snappedX = Math.round(relativeX / gridSize) * gridSize;
                let snappedY = Math.round(relativeY / gridSize) * gridSize;

                // Convert back to absolute position
                let finalX = snappedX + gridRect.left;
                let finalY = snappedY + gridRect.top;

                // Apply the snapped position
                target.style.left = `${finalX}px`;
                target.style.top = `${finalY}px`;

                target.dataset.x = finalX;
                target.dataset.y = finalY;

                target.style.zIndex = "10";
                selectedElement = target;
            }
        }
    });

    document.addEventListener("keydown", function(event) {
        if (event.key === "Delete" && selectedElement) {
            selectedElement.remove();
            selectedElement = null;
        }
    });
};

// Function to check if one element is over another
function isOverElement(draggableRect, staticRect) {
    return (
        draggableRect.top < staticRect.bottom &&
        draggableRect.bottom > staticRect.top &&
        draggableRect.left < staticRect.right &&
        draggableRect.right > staticRect.left
    );
}