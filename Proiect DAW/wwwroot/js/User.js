window.addEventListener("load", () => {
    
    let event = (() => {
        var source = document.getElementById("user-template").innerHTML;
        var template = Handlebars.compile(source);
        let noUsers = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: "/Users/GetUsers",
                    data: {
                        toSkip: noUsers
                    },
                    success: (result) => {

                        for (let i = 0; i < result.length; i++) {
                            let user = result[i];
                            var html = template(user);
                            $("#userBody").append(html);
                            $("#userBody tr:last-child a.needConfirmation").click(prevent);
                        }
                        noUsers += result.length;
                        canGet = true;
                        if (result.length == 0) canGet = false;
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
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight-1000) {
            event();
            event = () => { }
            this.setTimeout(() => {
                event = functionCopy;
            }, 1000)
        }
    })
    let filtrare = () => {
        let value = $("#selectUsers").val();
        
            x = 0;
            $.each($("#userBody tr"), (index, element) => {
                let row = $(element);
                if (row.find("td p").text().includes(value)) {
                    row.show();
                    x++;
                }
                else {
                    row.hide();
                }
            })
            
        
    };
    $("#selectUsers").keyup(filtrare);

    
})