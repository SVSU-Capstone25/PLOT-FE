/*
    Filename: authHelper.js
    Part of Project: PLOT/PLOT-FE/Services

    File Purpose:
    Provide workaround JS functions to set a cookie using a httpost request,
    and to be able to get the cookie in browser. Both used by auth

    Written by: Michael Polhill
*/


//Function to send a httprequest with a email and password message 
window.loginUser = async function (url, email, password) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            credentials: 'include', 
            body: JSON.stringify({ email: email, password: password })
        });
        
        return response.ok;
    } catch (error) {
        console.error('Login error:', error);
        return false;
    }
}

//Function to send a httprequest to logout a user
window.logoutUser = async function (url, token) {
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`, 
                'Content-Type': 'application/json'
            },
            credentials: 'include', 
        });
        
        return response.ok;
    } catch (error) {
        console.error('LogOut error:', error);
        return false;
    }
}


//Function to get a cookie by name from the browser
window.getCookie = function (name) {
    let nameEQ = name + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) === 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

// window.captureFloorsetThumbnail = async (floorsetId) => {
//     if (!window.p5Instance || !window.gridInstance || !window.p5Instance.canvas) {
//         console.error("Missing p5 instance or grid!");
//         return;
//     }

//     const canvas = window.p5Instance.canvas;
//     const grid = window.gridInstance;

//     // Step 1: Save current zoom + position
//     const originalScale = grid.scale;
//     const originalTranslate = { ...grid.translate };

//     // Step 2: Zoom out to scale 1 (or any value that fits all fixtures)
//     grid.scale = 1;
//     grid.resize(); // recompute translate

//     // Step 3: Redraw once with zoomed out view
//     window.p5Instance.redraw();

//     // Step 4: Wait for the draw to complete before capturing
//     await new Promise(resolve => setTimeout(resolve, 100)); // 1 frame (~100ms)

//     const dataUrl = canvas.toDataURL("image/png");

//     // Step 5: Restore previous zoom and translate
//     grid.scale = originalScale;
//     grid.translate = originalTranslate;
//     grid.resize();
//     window.p5Instance.redraw();

//     // Step 6: Trigger download (or send to backend later)
//     const link = document.createElement('a');
//     link.href = dataUrl;
//     link.download = `floorset_${floorsetId}.png`;
//     document.body.appendChild(link);
//     link.click();
//     document.body.removeChild(link);

//     console.log("Thumbnail captured and zoom restored.");
// };

// window.captureFloorsetThumbnailClean = async (floorsetId) => {
//     const width = 300;
//     const height = 300;

//     const sketch = (p5) => {
//         p5.setup = () => {
//             p5.createCanvas(width, height);
//             p5.background(255);

//             // Create a fresh grid instance
//             const grid = new Grid(p5);
//             grid.width = window.gridInstance.width;
//             grid.height = window.gridInstance.height;
//             grid.racks = window.gridInstance.racks; // reuse existing rack data
//             grid.scale = Math.min(width / (grid.width * grid.size), height / (grid.height * grid.size));
//             grid.resize();

//             // Draw full layout at calculated scale
//             p5.push();
//             grid.draw();
//             p5.pop();

//             // Capture and download image
//             setTimeout(() => {
//                 const dataUrl = p5.canvas.toDataURL('image/png');
//                 const link = document.createElement('a');
//                 link.href = dataUrl;
//                 link.download = `floorset_${floorsetId}.png`;
//                 link.click();

//                 // Cleanup (remove canvas)
//                 p5.remove();
//             }, 100);
//         };
//     };

//     // Create offscreen container and render
//     const container = document.createElement('div');
//     container.style.position = 'absolute';
//     container.style.left = '-10000px';
//     document.body.appendChild(container);
//     new p5(sketch, container);
// };
