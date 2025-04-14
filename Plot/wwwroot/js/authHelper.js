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
