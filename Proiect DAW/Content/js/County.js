window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("county-template").innerHTML;
        var template = Handlebars.compile(source);
        let noCounties = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: "/Counties/GetCounties",
                    data: {
                        toSkip: noCounties
                    },
                    success: (result) => {
                        noCounties += result.length;
                        for (let i = 0; i < result.length; i++) {
                            let county = result[i];
                            var html = template(county);
                            $("#countyBody").append(html);

                            $("a.needConfirmation").unbind("click").click((e) => {
                                var x = confirm("Are you sure of your action?");
                                if (!x) {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    e.stopImmediatePropagation();
                                    return;
                                }
                                let val = e.currentTarget.dataset.id;
                                $.ajax({
                                    type: "GET",
                                    url: "/Counties/CanDelete",
                                    data: {
                                        countyId: val
                                    },
                                    success: (result) => {
                                        if (!result) {
                                            alert("You can't delete this county");

                                        }
                                        else {

                                            alert("You deleted it");
                                            window.location.href = "/Counties/Index"
                                        }
                                    }

                                })
                            })


                        }
                        canGet = true;
                        if (result.length === 0) canGet = false;
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