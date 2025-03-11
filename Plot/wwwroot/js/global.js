// Danielle Smith - 3/10/2025
// When a dropdown is loaded, call a global JS function 
// to initialize it properly with Bootstrap
window.reinitializeDropdownById = (dropdownId) => {
    // get the element by it's ID
    let el = document.getElementById(dropdownId);
    if (!el) {
        console.warn(`Dropdown with ID '${dropdownId}' not found.`);
        return;
    }

    // get the dropdown button
    let button = el.querySelector('[data-bs-toggle="dropdown"]');
    if (!button) {
        console.warn(`Dropdown button inside '${dropdownId}' not found.`);
        return;
    }

    // if there is an existing instance, destroy it
    let dropdownInstance = bootstrap.Dropdown.getInstance(button);
    if (dropdownInstance) {
        dropdownInstance.dispose(); 
    }

    // initialize bootstrap dropdown
    let newDropdown = new bootstrap.Dropdown(button);
    button.addEventListener("click", () => {
        newDropdown.toggle();
    });
};
