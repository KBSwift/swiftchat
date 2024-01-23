function showAlert(message, type) {
    const alertPlaceholder = document.getElementById('alertPlaceholder');
    const alert = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                      ${message}
                      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                   </div>`;
    alertPlaceholder.innerHTML = alert;

    setTimeout(() => {
        alertPlaceholder.innerHTML = '';
    }, 2000);
}

function showSuccess(message) {
    showAlert(message, 'success');
}

function showError(message) {
    showAlert(message, 'danger');
}
