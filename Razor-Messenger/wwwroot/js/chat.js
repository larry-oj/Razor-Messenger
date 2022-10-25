﻿"use strict";

const chat = document.getElementById("messages-area");
const sendButton = document.getElementById("new-message-btn");
const messageInput = document.getElementById("new-message-text");
const sender = document.getElementById("sender").value;

function scrollToBottom() {
    chat.scrollTop = chat.scrollHeight;
}
scrollToBottom();

function messageBuilder(message, messageTime, isSender) {
    let styles = isSender ? "bg-primary text-white align-self-end" : "bg-light border text-dark";
    return `<div class="p-2 mb-1 d-flex flex-column rounded align-items-end ${styles}" style="width: fit-content; min-width: 10%; max-width: 75%;">     <p class="mb-0 text-wrap">${message}</p>     <small>${messageTime}</small> </div>`
}

let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

sendButton.disabled = true;

connection.on("ReceiveMessage", function (messageSender, message, messageTime) {
    let receiver = document.getElementById("receiver").value;
    if (messageSender !== receiver) return
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
    
    let receiver = document.getElementById("receiver").value;
    
    connection.invoke("SendMessage", receiver, message).catch(function (err) {
        return console.error(err.toString());
    });

    messageInput.value = "";
    event.preventDefault();
});