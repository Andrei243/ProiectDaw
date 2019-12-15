window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("locality-template").innerHTML;
        var template = Handlebars.compile(source);
        let noLocalities = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: "/Localities/GetLocalities",
                    data: {
                        toSkip: noLocalities
                    },
                    success: (result) => {

                        for (let i = 0; i < result.length; i++) {
                            let locality = result[i];
                            var html = template(locality);
                            $("#localityBody").append(html);
                            $("#localityBody tr:last-child a.needConfirmation").click(prevent);
                        }
                        noLocalities += result.length;
                        canGet = true;
                        if (result.length === 0) {
                            canGet = false;
                        }
                    }
                })
            }

        }
    })();
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

})