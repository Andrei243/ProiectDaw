$("#sendMessageToUserButton").click((e) => {
    let message = $("#sendMessageToUser").val();
    let id = $("#sentUserId").data("userid");
    $.ajax({
        type: "POST",
        url: "/Message/SendMessageToPerson",
        data: {
            message: message,
            userId: id
        },
        success: function (response) {
            location.reload(true);
        }
    })
    
})