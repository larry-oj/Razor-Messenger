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

function messageBuilder(message, messageTime, isSender, id) {
    let styles = isSender ? "bg-primary text-white align-self-end" : "bg-light border text-dark";
    return `<div id="message-${id}" class="p-0 mb-1 d-flex flex-row rounded align-items-end justify-content-between ${styles}" style="width: fit-content; min-width: 10%; max-width: 75%;">     <div class="p-2 d-flex flex-column align-items-end">         <p class="mb-0 text-wrap">${message}</p>         <small>${messageTime}</small>     </div> </div>`
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

connection.on("ReceiveMessage", function (messageSender, message, messageTime, messageId) {
    if (messageSender !== receiver.value) return
    let messageContainer = messageBuilder(message, messageTime, false, messageId);
    chat.innerHTML += messageContainer;
    if (chat.scrollTop < 400) {
        scrollToBottom();
    }
});

connection.on("SendMessage", function (message, messageTime, messageId) {
    let messageContainer = messageBuilder(message, messageTime, true, messageId);
    chat.innerHTML += messageContainer;
    scrollToBottom();
});

connection.on("ReceiveEmotionAnalysis", function (messageId, emotion, color) {
    let message = document.getElementById(`message-${messageId}`);
    console.log(message);
    if (!message) return;
    message.innerHTML = `<div title="${emotion}" class="p-0 m-0 h-100 rounded" style="background-color: ${color}; width: 5px"></div>` + message.innerHTML;
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