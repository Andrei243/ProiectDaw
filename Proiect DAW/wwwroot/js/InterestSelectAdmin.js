window.addEventListener("load", () => {


    $.ajax({

        type: 'GET',
        url: '/Users/GetInterests',
        data: {
            userId: $("#Id").attr("value")
        },
        success: (result) => {
            $("#interestSelect").select2({
                data: result,
                multiple: true
            })
        }
    });

    $("#submit").click((e) => {
        $("#interestSelect *").removeAttr("selected");
        $(":selected").attr("selected", "selected");


    })



})