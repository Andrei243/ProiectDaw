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

$("#sendMessageToGroupButton").click((e) => {
    let message = $("#sendMessageToGroup").val();
    let id = $("#sentGroupId").data("userid");
    $.ajax({
        type: "POST",
        url: "/Message/SendMessageToGroup",
        data: {
            message: message,
            groupId: id
        },
        success: function (response) {
            location.reload(true);
        }
    })

})