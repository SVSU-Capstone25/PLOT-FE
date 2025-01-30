window.dragDrop = function(className) {
    const position = { x: 0, y: 0 }

    interact(className).draggable({
            autoScroll: true,
            listeners: {
                start(event) {
                    const target = event.target;
                    console.log("start");
                    // Set a high z-index while dragging
                    target.style.zIndex = "1000";

                    // Ensure it has absolute positioning
                    target.style.position = "absolute";
                    
                    // Store initial position
                    target.dataset.x = target.dataset.x || 0;
                    target.dataset.y = target.dataset.y || 0;
                },
                move(event) {
                    const target = event.target;
                    // Get stored position or default to (0,0)
                    let x = (parseFloat(target.dataset.x) || 0) + event.dx;
                    let y = (parseFloat(target.dataset.y) || 0) + event.dy;

                    // Apply transform correctly
                    target.style.transform = `translate(${x}px, ${y}px)`;

                    // Store the new position
                    target.dataset.x = x;
                    target.dataset.y = y;
                },
                end(event) {
                    // Reset z-index after dragging
                    event.target.style.zIndex = "10";
                    console.log("end");
                }
            }
        });

};