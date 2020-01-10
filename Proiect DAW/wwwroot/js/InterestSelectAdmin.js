window.addEventListener("load", () => {


    $.ajax({

        type: 'GET',
        url: '/Users/GetInterests',
        data: {
            userId: $("#Id").attr("value")
        },
        success: (result) => {
            for (let i = 0; i < result.length; i++) {
                result[i].selected = result[i].Selected;
                result[i].id = result[i].Id;
                result[i].text = result[i].Text;
            }
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