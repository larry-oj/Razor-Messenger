"use strict";

let userListConn = new signalR.HubConnectionBuilder().withUrl("/userListHub").build();

userListConn.on("UpdateLastMessage", function (username, message, messageTime) {
    document.getElementById(`userlist-message-${username}`).innerText = message;
    document.getElementById(`userlist-time-${username}`).innerText = messageTime;
});

userListConn.start().catch(function (err) {
    return console.error(err.toString());
});