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