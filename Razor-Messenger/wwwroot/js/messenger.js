function selectUser(username) {
    console.log("execute - " + username);
    $.ajax({
        url: "/Messenger?handler=SelectUser",
        type: 'POST',
        data: { receiver: username },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("X-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data)
        {
            let receiver = $("#receiver");
            let old = receiver.val();
            if (old !== "") {
                $(`#userlist-${old}`).removeClass("bg-light");
            }
            $(`#userlist-${username}`).addClass('bg-light');
            receiver.val(username);
            $("#messages-area").html(data);
            scrollToBottom();
        },
        failure: function ()
        {
            alert("failure");
        }
    });
}

document.getElementById("userlist-search-input").oninput = searchUsers;
function searchUsers() {
    let query = $("#userlist-search-input").val();
    console.log(query);
    $.ajax({
        url: "/Messenger?handler=SearchUsers",
        type: 'POST',
        data: { query: query },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("X-XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (data)
        {
            $("#userlist").html(data);
            
            let usernames = getUsernames();
            userListConn.invoke("GetOnlineUsers", usernames).catch(function (err) {
                console.error(err.toString());
            });
        },
        failure: function ()
        {
            alert("failure");
        }
    });
}