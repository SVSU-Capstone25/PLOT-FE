window.dragDrop = function(className, sidebarSelector) {
    const position = { x: 0, y: 0 }

    let selectedElement = null;

    interact(className).draggable({
        autoScroll: true,
        listeners: {
            start(event) {
                const target = event.target;
                
                // Check if the element is from the sidebar or the grid
                const isFromSidebar = target.closest(sidebarSelector) !== null;

                if (isFromSidebar) {
                    // Clone the element if it's from the sidebar
                    const clonedElement = target.cloneNode(true);
                    clonedElement.id = "dragged-" + Date.now(); // Ensure it has a unique ID

                    // Apply initial styles to the cloned element
                    clonedElement.style.position = "absolute";
                    clonedElement.style.zIndex = "1000";
                    clonedElement.style.opacity = "1"; // Full opacity for the clone
                    
                    // Add the clone to the body, so it floats above other elements
                    document.body.appendChild(clonedElement);

                    // Store the initial position of the clone (centered on the original element)
                    const rect = target.getBoundingClientRect();
                    const offsetX = event.clientX - rect.left;
                    const offsetY = event.clientY - rect.top;

                    clonedElement.dataset.x = 0;
                    clonedElement.dataset.y = 0;

                    // Set the clone to appear at the center of the original element
                    clonedElement.style.left = event.clientX - offsetX + 'px';
                    clonedElement.style.top = event.clientY - offsetY + 'px';

                    // Store the clone's unique ID in the event target's dataset
                    event.target.dataset.clonedId = clonedElement.id;

                    // Ensure the original remains semi-transparent
                    target.style.opacity = "0.8";
                } else {
                    // When dragging from the grid, no cloning, just move the original element
                    target.style.zIndex = "1000"; // Bring it to the front
                    target.style.opacity = "1"; // Full opacity for the original element
                }
                selectedElement = target;
            },
            move(event) {
                const target = event.target;

                // Check if the element was cloned
                const clonedElement = document.getElementById(target.dataset.clonedId);

                if (clonedElement) {
                    let x = (parseFloat(clonedElement.dataset.x) || 0) + event.dx;
                    let y = (parseFloat(clonedElement.dataset.y) || 0) + event.dy;

                    // Apply the new position to the clone
                    clonedElement.style.transform = `translate(${x}px, ${y}px)`;

                    // Store the new position in the cloned element
                    clonedElement.dataset.x = x;
                    clonedElement.dataset.y = y;
                } else {
                    // Otherwise, move the original element (if not cloned)
                    let x = (parseFloat(target.dataset.x) || 0) + event.dx;
                    let y = (parseFloat(target.dataset.y) || 0) + event.dy;

                    // Apply the new position to the original element
                    target.style.transform = `translate(${x}px, ${y}px)`;

                    // Store the new position in the original element
                    target.dataset.x = x;
                    target.dataset.y = y;
                }
            },
            end(event) {
                const target = event.target;
                const clonedElement = document.getElementById(target.dataset.clonedId);

                // Get the sidebar element
                const sidebar = document.querySelector(sidebarSelector);

                // Check if the cloned element is over the sidebar
                if (clonedElement) {
                    const clonedRect = clonedElement.getBoundingClientRect();
                    const sidebarRect = sidebar.getBoundingClientRect();

                    // If the cloned element is still over the sidebar, cancel the operation
                    if (
                        clonedRect.top < sidebarRect.bottom &&
                        clonedRect.bottom > sidebarRect.top &&
                        clonedRect.left < sidebarRect.right &&
                        clonedRect.right > sidebarRect.left
                    ) {
                        // Cancel the drag-drop operation by removing the clone
                        clonedElement.remove();

                        // Optionally, reset the opacity of the original element
                        target.style.opacity = "0.8";
                    } else {
                        // Otherwise, reset z-index and opacity of the clone
                        clonedElement.style.zIndex = "10"; // Reset z-index to default
                        clonedElement.style.opacity = "0.8"; // Make the clone semi-transparent after drop
                    }
                }

                // If the element is from the grid (not cloned), reset opacity and z-index
                if (!clonedElement) {
                    target.style.zIndex = "10"; // Reset z-index
                    target.style.opacity = "1"; // Full opacity
                }
            }
        }
    });

    // Add a listener for the Delete key
    document.addEventListener('keydown', function(event) {
        // Check if the Delete key is pressed (key code for Delete is 46)
        if (event.key === "Delete" && selectedElement) {
            // Remove the selected element from the grid
            selectedElement.remove();
            selectedElement = null; // Reset the selected element
        }
    });
};
