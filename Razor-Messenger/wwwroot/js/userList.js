"use strict";

let userListConn = new signalR.HubConnectionBuilder().withUrl("/userListHub").build();

userListConn.on("UpdateLastMessage", function (username, message, messageTime, isSender) {
    document.getElementById(`userlist-message-${username}`).innerText = (isSender ? "You: " : "") + message;
    document.getElementById(`userlist-time-${username}`).innerText = messageTime;
});

userListConn.on("GetOnlineUsers", function (usernames) {
    console.log(usernames);
    usernames.forEach(username => {
        document.getElementById(`userlist-onlinestatus-${username}`)
            .classList.add("online");
    });
});

userListConn.on("UpdateOnlineStatus", function (username, isOnline) {
    console.log(username, isOnline);
    let user = document.getElementById(`userlist-onlinestatus-${username}`);
    if (isOnline) {
        user.classList.add("online");
    } else {
        user.classList.remove("online");
    }
}); 

userListConn.start().then(function () { 
    userListConn.invoke("GetOnlineUsers").catch(function (err) {
        return console.error("yep - " + err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});