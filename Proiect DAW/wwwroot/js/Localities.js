var url = '/Account/GetLocalities';
$(function () {
    console.log($('#CountyId').val());

    $.ajax({
        type: 'GET',
        url: url,
        data: $('#CountyId').val(),
        success: function (response) {
            console.log(response);
            var elements = $.map(response, function (item) {
                return `<option value="${item.value}">${item.text}</option>`
            });
            $('#LocalityId').empty();
            $('#LocalityId').append(elements);
        },
        error: function (error) {
            console.log(error);
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
                    return `<option value="${item.value}">${item.text}</option>`
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