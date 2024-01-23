
// Handling HTTP Errors
function handleHttpError(response) {
    if (response.status === 400) {
        return 'Invalid input. Please check your data.';
    } else if (response.status === 404) {
        return 'Page not found.';
    } else if (response.status === 500) {
        return 'An internal server error occurred. Please try again later.';
    } else {
        return 'An error occurred while processing your request.';
    }
}