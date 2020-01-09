$(function () {
    html = "";

    $('#selection').keyup(function () {
        var name = $('#selection').val();
        $.ajax({
            type: 'GET',
            url: '/Users/GetUsersByName',
            data: {
                name: name
            },
            success: function (response) {
                $("#users").empty();
                html = '';
                for (let i = 0; i < response.length; i++) {

                    var redirect = '/Profile/Profile' + "?userId=" + response[i].Id;
                    var photoURL = '/Photos/Download' + "/" + response[i].ProfilePhotoId;
                    if (!!response[i].profilePhotoId) {
                        html = html.concat(`<a href=${redirect}><div class="dropdownItem"><p>${response[i].Name}</p><img src=${photoURL} height="100em" />   </div></a>`)
                    }
                    else {
                        html = html.concat(`<a href=${redirect}><div class="dropdownItem"><p>${response[i].Name}</p><img src="/images/DefaultProfile.png" height="100em" />   </div></a>`)
                    }

                }
                if (response.length === 0) {
                    html = '';
                }
                $("#users").append(html);
                $("#divSelection .dropdownDiv").show();
            },
            error: function (error) {

            }

        })
    })
    $("#selection").blur(() => {        
            $("#users").empty();
        $("#divSelection .dropdownDiv").hide();
    })

    $('#users').on('mousedown', function (event) {
        event.preventDefault();
    });

    $("#selection").focus(() => {
        $("#users").empty();
        $("#users").append(html);
        $("#divSelection .dropdownDiv").show();
    })


});