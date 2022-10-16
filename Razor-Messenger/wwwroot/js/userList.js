"use strict";

let userListConn = new signalR.HubConnectionBuilder().withUrl("/userListHub").build();

userListConn.on("UpdateLastMessage", function (username, message, messageTime) {
    document.getElementById(`userlist-message-${username}`).innerText = message;
    document.getElementById(`userlist-time-${username}`).innerText = messageTime;
});

const onlineStatus = `<small class="online">●</small>`;

userListConn.on("GetOnlineUsers", function (usernames) {
    console.log(usernames);
    usernames.forEach(username => {
        document.getElementById(`userlist-name-${username}`).innerHTML += onlineStatus;
    });
});

userListConn.on("UpdateOnlineStatus", function (username, isOnline) {
    console.log(username, isOnline);
    let user = document.getElementById(`userlist-name-${username}`);
    if (isOnline && !user.innerHTML.includes(onlineStatus)) {
        user.innerHTML += onlineStatus;
    } else {
        user.innerHTML = user.innerHTML.replaceAll(onlineStatus, "");
    }
}); 

userListConn.start().then(function () { 
    userListConn.invoke("GetOnlineUsers").catch(function (err) {
        return console.error("yep - " + err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});