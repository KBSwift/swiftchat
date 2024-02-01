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
    connection.on("ReceiveMessage", function (username, message, messageId, upvotes, downvotes, saveCount) {
        let msgContainer = document.createElement("div");
        msgContainer.classList.add("message-container");
        msgContainer.setAttribute('data-message-id', messageId); // Set the message ID as a data attribute
        msgContainer.innerHTML = `
        <span>${username}: ${message}</span>
        <span class="message-buttons">
            <button class="vote-button btn btn-outline-success btn-sm" data-message-id="${messageId}">
                <span style="line-height: 1;">
                    ${upvotes}<i class="bi bi-arrow-up"></i>
                </span>
                <span style="line-height: 1;">Like</span>
            </button>

            <button class="vote-button btn btn-outline-danger btn-sm" data-message-id="${messageId}">
                <span style="line-height: 1;">
                    ${downvotes}<i class="bi bi-arrow-down"></i>
                </span>
                <span style="line-height: 1;">Dislike</span>
            </button>
        
            <button class="save-button btn btn-outline-primary btn-sm" data-message-id="${messageId}">
                <i class="bi bi-bookmark"></i> Save
            </button>
        </span>`;
        document.getElementById("messageArea").appendChild(msgContainer);
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

    connection.on("ReceiveMessageUpdate", function (messageId, upvotes, downvotes, saveCount) {
        // Find the message in the message area using messageId
        // Update the displayed upvote, downvote, and save counts
        const messageElement = document.querySelector(`[data-message-id="${messageId}"]`);
        if (messageElement) {
            const upvoteElement = messageElement.querySelector('.upvote-count');
            const downvoteElement = messageElement.querySelector('.downvote-count');
            const saveCountElement = messageElement.querySelector('.save-count');

            if (upvoteElement) upvoteElement.textContent = upvotes;
            if (downvoteElement) downvoteElement.textContent = downvotes;
            if (saveCountElement) saveCountElement.textContent = saveCount;
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
const messageArea = document.getElementById("messageArea");

messageArea.addEventListener("onhover", function () {

});

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



