$(function () {
    let url = '/Account/GetLocalities';


    $.ajax({
        type: 'GET',
        url: url,
        data: { countyId: $('#CountyId').val() },
        success: function (response) {
            var elements = $.map(response, function (item) {
                return `<option value="${item.Value}">${item.Text}</option>`
            });
            $('#LocalityId').empty();
            $('#LocalityId').append(elements);
        },
        error: function (error) {
            console.error(error);
        }

    })

    $('#CountyId').on('change', function () {

        $.ajax({
            type: 'GET',
            url: url,
            data: {
                countyId: $('#CountyId').val()
            },
            success: function (response) {
                var elements = $.map(response, function (item) {
                    return `<option value="${item.Value}">${item.Text}</option>`
                });
                $('#LocalityId').empty();
                $('#LocalityId').append(elements);
            },
            error: function (error) {
                console.error(error);

            }

        })
    })
});