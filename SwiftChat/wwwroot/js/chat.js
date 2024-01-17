let connection;

function initializeChatSignalR() {
    // Create a connection to the SignalR hub
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    // Start SignalR connection
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    // Disable the send button until the connection is established.
    document.getElementById("sendButton").disabled = true;

    // Handling incoming messages
    connection.on("ReceiveMessage", function (username, message) {
        let msg = document.createElement("div");
        msg.textContent = `${username}: ${message}`;
        document.getElementById("messageArea").appendChild(msg);
    });

    // Send button actions and clicking
    document.getElementById("sendButton").addEventListener("click", function (event) {
        sendMessage();
        event.preventDefault();
    });

    // Handling Enter keypress in messageInput
    document.getElementById("messageInput").addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            event.preventDefault(); // Prevent default to stop new line creation
            sendMessage();
        }
    });
}

function sendMessage() {
    let message = document.getElementById("messageInput").textContent.trim();
    const placeholderText = "What I want to say is ....";

    if (message !== "" && message !== placeholderText) {
        // Send message to server
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        document.getElementById("messageInput").textContent = ''; // Clearing message box for next input
    }
}

// ChatHub input field behavior handling
document.addEventListener("DOMContentLoaded", function () {
    const messageInput = document.getElementById("messageInput");
    const placeholderText = "What I want to say is ....";

    function updatePlaceholder() {
        if (messageInput.textContent.trim() === "") {
            messageInput.innerHTML = placeholderText;
            messageInput.classList.add("message-placeholder");
        } else {
            messageInput.classList.remove("message-placeholder");
        }
    }

    updatePlaceholder();

    // When focusing on text
    messageInput.addEventListener("focus", function () {
        if (messageInput.textContent.trim() === placeholderText) {
            messageInput.textContent = "";
            messageInput.classList.remove("message-placeholder");
        }
    });

    // Blur event
    messageInput.addEventListener("blur", updatePlaceholder);
});


