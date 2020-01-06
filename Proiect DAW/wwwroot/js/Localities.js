$(function () {
    let url = '/Account/GetLocalities';

    console.log($('#CountyId').val());

    $.ajax({
        type: 'GET',
        url: url,
        data: { countyId: $('#CountyId').val() },
        success: function (response) {
            console.log("Am intrat");
            console.log(response);
            var elements = $.map(response, function (item) {
                return `<option value="${item.Value}">${item.Text}</option>`
            });
            $('#LocalityId').empty();
            $('#LocalityId').append(elements);
        },
        error: function (error) {
            console.log(error);
        }

    })

    $('#CountyId').on('change', function () {
        console.log($('#CountyId').val());

        $.ajax({
            type: 'GET',
            url: url,
            data: {
                countyId:$('#CountyId').val() 
            },
            success: function (response) {
                console.log("Am intrat");
                console.log(response);
                var elements = $.map(response, function (item) {
                    return `<option value="${item.Value}">${item.Text}</option>`
                });
                $('#LocalityId').empty();
                $('#LocalityId').append(elements);
            },
            error: function (error) {
                console.log(error);

            }

        })
    })
});