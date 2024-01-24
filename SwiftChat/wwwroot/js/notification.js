function showAlert(message, time, type) {
    const alertPlaceholder = document.getElementById('alertPlaceholder');
    const alert = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                      ${message}
                      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                   </div>`;
    alertPlaceholder.innerHTML = alert;

    setTimeout(() => {
        alertPlaceholder.innerHTML = '';
    }, time);
}

function showSuccess(message, time = 3000) {
    showAlert(message, time, 'success');
}

function showError(message, time = 3000) {
    showAlert(message, time, 'danger');
}
