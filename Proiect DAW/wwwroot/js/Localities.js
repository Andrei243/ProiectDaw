var url = '/Account/GetLocalities';
$(function () {
    console.log($('#CountyId').val());
    $.ajax({
        type: 'GET',
        url: url,
        data: JSON.stringify({ countyId: $('#CountyId').val() }),
        dataType: "json",
        contentType:"application/json;charset=utf-8",
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
    console.log($('#CountyId').val());

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
    console.log($('#CountyId').val());


});