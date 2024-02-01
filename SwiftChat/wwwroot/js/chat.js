let connection;
const placeholderText = "What I want to say is ....";

function initializeChatSignalR() {
    // Create a connection to the SignalR hub
    connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    // Start SignalR connection
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    // Disable the send button until the connection is established
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
    let messageInput = document.getElementById("messageInput");
    let message = messageInput.textContent.trim();
    const isError = messageInput.getAttribute("data-is-error") === "true";

    if ((message === "" || message === placeholderText) && !isError) {
        messageInput.textContent = "Please enter a message here first";
        messageInput.classList.add("error");
        messageInput.setAttribute("data-is-error", "true");
    } else if (!isError) {
        // Send message to server
        connection.invoke("SendMessage", message).catch(function (err) {
            return console.error(err.toString());
        });
        messageInput.textContent = "";
        messageInput.setAttribute("data-is-error", "false");
    }
}

document.getElementById("messageInput").addEventListener("focus", function () {
    let messageInput = document.getElementById("messageInput");
    if (messageInput.getAttribute("data-is-error") === "true") {
        messageInput.textContent = "";
        messageInput.classList.remove("error");
        messageInput.setAttribute("data-is-error", "false");
        messageInput.setAttribute("is-data-placeholder", "false");
    }
});

document.getElementById("messageInput").addEventListener("input", function () {
    let messageInput = document.getElementById("messageInput");
    if (messageInput.getAttribute("data-is-error") === "true") {
        messageInput.textContent = "";
        messageInput.classList.remove("error");
        messageInput.setAttribute("data-is-error", "false");
        messageInput.setAttribute("is-data-placeholder", "false");
    }
});


// ChatHub input field behavior handling

const messageInput = document.getElementById("messageInput");

function updatePlaceholder() {
    if (messageInput.textContent.trim() === "") {
        messageInput.innerHTML = placeholderText;
        messageInput.classList.add("message-placeholder");
        messageInput.setAttribute("is-data-placeholder", "true");
    } else {
        messageInput.classList.remove("message-placeholder");
        messageInput.setAttribute("is-data-placeholder", "false");
    }
}

updatePlaceholder();

// When focusing on text
messageInput.addEventListener("focus", function () {
    updatePlaceholder();
    if (messageInput.textContent.trim() === placeholderText) {
        messageInput.textContent = "";
        messageInput.classList.remove("message-placeholder");
    }
});

// Blur event
messageInput.addEventListener("blur", updatePlaceholder);



