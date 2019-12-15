window.addEventListener("load", () => {
    let changeDescription = (e) => {
        let nod = e.currentTarget;
        let text = e.currentTarget.parentNode.querySelector(".description").value;
        let photoId = e.currentTarget.dataset.photoid;
        $.ajax({
            type: "PUT",
            url: "/Profile/ChangeDescription",
            data: {
                photoId: parseInt(photoId),
                description: text
            },
            success: (response) => {
                if (response) {
                    nod.parentNode.parentNode.querySelector("p").innerText = text;
                    nod.parentNode.parentNode.querySelector(".description").value = "";
                }
            }

        })


    }
    let albumIdc = document.getElementById("imageGetter").dataset.album;
    let event = ((albumId) => {
        var source = document.getElementById("image-template").innerHTML;
        var template = Handlebars.compile(source);
        let noImages = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: "/Profile/GetPhotosJson",
                    data: {
                        toSkip: noImages,
                        albumId: albumId
                    },
                    success: (result) => {
                        for (let i = 0; i < result.length; i++) {

                            let image = result[i];
                            var html = template(image);
                            $("#imageBody").append(html);
                            $("#changeDescription" + image.id).click(changeDescription);
                            $("#changeDescription" + image.id).click(prevent);
                            $("#makeProfile" + image.id).click(prevent);
                            $("#removePhoto" + image.id).click(prevent);
                        }
                        noImages += result.length;
                        canGet = true;
                        if (result.length === 0) {
                            canGet = false;
                        }
                    }
                })
            }


        }
    })(albumIdc);
    
    event();
    let functionCopy = event;
    event = () => { };
    setTimeout(() => {
        event = functionCopy;
    }, 1000);
    $(window).scroll(() => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            event();
            event = () => { }
            this.setTimeout(() => {
                event = functionCopy;
            }, 1000)
        }

    })
    $("#imgPreview").attr("hidden", true);

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgPreview').attr('src', e.target.result);
                $("#imgPreview").attr("hidden", false);
            }

            reader.readAsDataURL(input.files[0]);
        }
        else {
            $("#imgPreview").attr("hidden", true);
        }
    }
    document.getElementById("Binar").addEventListener("change", function () {
        readURL(this);
    })
})