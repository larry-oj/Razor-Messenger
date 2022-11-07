"use strict";

const chat = document.getElementById("messages-area");
const sendButton = document.getElementById("new-message-btn");
const messageInput = document.getElementById("new-message-text");
const sender = document.getElementById("sender");
const receiver = document.getElementById("receiver");

function scrollToBottom() {
    chat.scrollTop = chat.scrollHeight;
}
scrollToBottom();

function messageBuilder(message, messageTime, isSender) {
    let styles = isSender ? "bg-primary text-white align-self-end" : "bg-light border text-dark";
    return `<div class="p-2 mb-1 d-flex flex-column rounded align-items-end ${styles}" style="width: fit-content; min-width: 10%; max-width: 75%;">     <p class="mb-0 text-wrap">${message}</p>     <small>${messageTime}</small> </div>`
}

function userlistBuilder(username, displayName, message, messageTime) {
    return `<a href="#" onclick="selectUser('${username}');event.preventDefault();" id="userlist-${username}" class="list-group-item" aria-current="true">
         <div class="d-flex w-100 justify-content-between">
             <h5 class="mb-1 nowrap">
                 <span id="userlist-name-${username}">${displayName}</span> <small id="userlist-onlinestatus-${username}" class="online-status online">●</small>
             </h5>
                 <small id="userlist-time-${username}" class="text-muted">${messageTime}</small>
         </div>
        <p id="userlist-message-${username}" class="text-muted mb-1 w-100 nowrap">
            ${message}
        </p>
     </a>`;
}

let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

sendButton.disabled = true;

connection.on("ReceiveMessage", function (messageSender, message, messageTime) {
    if (messageSender !== receiver.value) return
    let messageContainer = messageBuilder(message, messageTime, false);
    chat.innerHTML += messageContainer;
    if (chat.scrollTop < 400) {
        scrollToBottom();
    }
});

connection.on("SendMessage", function (message, messageTime) {
    let messageContainer = messageBuilder(message, messageTime, true);
    chat.innerHTML += messageContainer;
    scrollToBottom();
});

connection.start().then(function () {
    sendButton.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

sendButton.addEventListener("click", function (event) {
    let message = messageInput.value;
    if (message === "" || message === null) return;
    
    connection.invoke("SendMessage", receiver.value, message).catch(function (err) {
        return console.error(err.toString());
    });

    messageInput.value = "";
    event.preventDefault();
});