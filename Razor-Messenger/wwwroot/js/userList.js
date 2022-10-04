"use strict";

let userListConn = new signalR.HubConnectionBuilder().withUrl("/userListHub").build();

userListConn.on("UpdateLastMessage", function (username, message, messageTime) {
    document.getElementById(`userlist-message-${username}`).innerText = message;
    document.getElementById(`userlist-time-${username}`).innerText = messageTime;
});

userListConn.start().then(function () {
    userListConn.invoke("Register", sender).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});