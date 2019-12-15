window.addEventListener("load", () => {
    

    $.ajax({

        type: 'GET',
        url: '/Profile/GetInterests',
        data: {
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