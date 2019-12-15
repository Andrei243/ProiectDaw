window.addEventListener("load", () => {
    let event = (() => {
        var source = document.getElementById("friend-template").innerHTML;
        var template = Handlebars.compile(source);
        let noFriends = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: "/Profile/GetRequesters",
                    data: {
                        toSkip: noFriends
                    },
                    success: (result) => {

                        for (let i = 0; i < result.length; i++) {
                            let friend = result[i];
                            var html = template(friend);
                            $("#friendRequestBody").append(html);
                            $("#friendRequestBody div:last-child a.needConfirmation").click(prevent);
                        }
                        noFriends += result.length;
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