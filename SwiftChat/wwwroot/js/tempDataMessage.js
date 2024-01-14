function tempDataMessage(timeout = 2000) {
    const messageElement = document.getElementById('tempDataMessage');
    if (messageElement) {
        // Hide the message after the timeout
        setTimeout(function () {
            messageElement.style.display = 'none';
        }, timeout);

        // Hide the message immediately when the close button is clicked
        const closeButton = messageElement.querySelector('.close');
        if (closeButton) {
            closeButton.addEventListener('click', function () {
                messageElement.style.display = 'none';
            });
        }
    }
}
