"use strict";

let userListConn = new signalR.HubConnectionBuilder().withUrl("/userListHub").build();

userListConn.on("UpdateLastMessage", function (username, message, messageTime, isSender) {
    document.getElementById(`userlist-message-${username}`).innerText = (isSender ? "You: " : "") + message;
    document.getElementById(`userlist-time-${username}`).innerText = messageTime;
});

userListConn.on("GetOnlineUsers", function (usernames) {
    usernames.forEach(username => {
        let userDiv = document.getElementById(`userlist-onlinestatus-${username}`);
        if (userDiv != null || userDiv !== undefined) {
            userDiv.classList.add("online");
        }
    });
});

userListConn.on("UpdateOnlineStatus", function (username, isOnline) {
    let user = document.getElementById(`userlist-onlinestatus-${username}`);
    if (isOnline) {
        user.classList.add("online");
    } else {
        user.classList.remove("online");
    }
}); 

userListConn.on("UpdateDisplayName", function (username, displayName) {
    if (username === sender) return;
    let user = document.getElementById(`userlist-name-${username}`);
    user.innerText = displayName;
});

function getUsernames() {
    let usernameList = [];
    let users = document.getElementById("userlist").children;
    for (let i = 0; i < users.length; i++) {
        let id = users[i].id.replaceAll("userlist-", "");
        usernameList.push(id);
    }
    
    return usernameList;
}

userListConn.start().then(function () { 
    let usernames = getUsernames();
    
    userListConn.invoke("GetOnlineUsers", usernames).catch(function (err) {
        return console.error("yep - " + err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});