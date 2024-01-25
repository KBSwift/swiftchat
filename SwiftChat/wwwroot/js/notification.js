function showAlert(message, timeout, type) {
    const alertPlaceholder = document.getElementById("alertPlaceholder");
    const alert = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                      ${message}
                      
                   </div>`;
    alertPlaceholder.innerHTML = alert;

    setTimeout(() => {
        alertPlaceholder.innerHTML = "";
    }, timeout);
}

function showSuccess(message, timeout = 2000) {
    showAlert(message, timeout, "success");
}

function showError(message, timeout = 2000) {
    showAlert(message, timeout, "danger");
}


document.addEventListener("DOMContentLoaded", function () {
    if (successMessage && successMessage !== "") {
        showSuccess(successMessage);
    }
    if (errorMessage && errorMessage !== "") {
        showError(errorMessage);
    }
});