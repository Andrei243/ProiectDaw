window.addEventListener("load", function () {
    var sourceComment = document.getElementById("comment-template").innerHTML;
    var templateComment = Handlebars.compile(sourceComment);
    let eventLike = function (e) {

        $.ajax({
            type: 'PUT',
            url: '/Feed/Reaction',
            data: {
                postId: e.currentTarget.dataset.post
            },
            success: function (response) {
                if (response!="False") {
                    e.currentTarget.querySelector("img").src = "/images/Liked.png";
                    e.currentTarget.parentNode.querySelector(".reactionCounter").innerText = (parseInt(e.currentTarget.parentNode.querySelector(".reactionCounter").innerText) + 1)+" Likes";
                }
                else {
                    e.currentTarget.querySelector("img").src = "/images/notLiked.png";
                    e.currentTarget.parentNode.querySelector(".reactionCounter").innerText = (parseInt(e.currentTarget.parentNode.querySelector(".reactionCounter").innerText) - 1)+" Likes";
                }
            },
            error: function (error) {
                console.log(error)
            }
        }

        )

    };

    let eventDeleteComment = function (e) {
        let com = $(this).parent().parent();
        
        $.ajax({
            type: "DELETE",
            url: '/Feed/RemoveComment',
            data: {
                commentId: e.currentTarget.dataset.comment,

            },
            success: function (response) {
                com.parent().data("toskip", parseInt(com.parent().data("toskip")) - 1);
                com.remove();
            },
            error: function (error) {
                console.log(error);
            }


        })

    };

    let eventAddComment = function (e) {
        let postId = e.currentTarget.dataset.post;
        $.ajax({
            type: "POST",
            url: '/Feed/Comment',
            data: {
                postId: postId,
                comentariu: e.currentTarget.parentNode.querySelector("input").value
            },
            success: function (response) {
                if (response!==-1) {
                    let obj = {
                        Text: e.currentTarget.parentNode.querySelector("input").value,
                        Id: response,
                        IsMine: true,
                        UserId: $("#currentUserInfo").data("userid"),
                        ProfilePhoto: $("#currentUserInfo").data("profile"),
                        UserName: $("#currentUserInfo").data("name")
                    }
                    let html = templateComment(obj);
                    $("#commentBody" + postId).prepend(html);
                    $("#commentBody" + postId).data("toskip", parseInt($("#commentBody" + postId).data("toskip")) + 1);
                    $("#deleteComment" + response).click(prevent);
                    $("#deleteComment" + response).click(eventDeleteComment);
                    e.currentTarget.parentNode.querySelector("input").value = "";

                }
                else {
                    alert("The comment wasn't added");
                }
            },
            error: function (error) {
                console.log(error);
            }


        })

    };

    
    let eventDeletePost = function (e) {
        let post = $(this).parent().parent();
        $.ajax({
            type: "DELETE",
            url: '/Feed/RemovePost',
            data: {
                postId: e.currentTarget.dataset.post,

            },
            success: function (response) {
                post.remove();
            },
            error: function (error) {
                console.log(error);
            }


        })

    }

    

   
    let eventComment = (idPost) => {

        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: '/Feed/GetComments',
                    data: {
                        toSkip: $("#commentBody" + idPost).data("toskip"),
                        postId: idPost
                    },
                    success: (result) => {
                        $("#commentBody" + idPost).data("toskip", parseInt($("#commentBody" + idPost).data("toskip")) + result.length);
                        for (let i = 0; i < result.length; i++) {
                            let comment = result[i];
                            let html = templateComment(comment);
                            $("#commentBody" + idPost).append(html);
                            $("#deleteComment" + comment.Id).click(prevent);
                            $("#deleteComment" + comment.Id).click(eventDeleteComment);
                        }
                        canGet = true;

                        if (result.length === 0) {
                            $("#commentGetter" + idPost).remove();
                        }
                    }
                })
            }
        }

    };

    
    let eventPost = ((userId) => {
        var sourcePost = document.getElementById("post-template").innerHTML;
        var templatePost = Handlebars.compile(sourcePost);
        let noPosts = 0;
        let canGet = true;
        return () => {
            if (canGet) {
                canGet = false;
                $.ajax({
                    type: 'GET',
                    url: '/Feed/GetPersonPosts',
                    data: {
                        toSkip: noPosts,
                        userId:userId
                    },
                    success: (result) => {

                        noPosts += result.length;
                        for (let i = 0; i < result.length; i++) {
                            let post = result[i];
                            let html = templatePost(post);
                            $("#postBody").append(html);
                            let eventComentariu = eventComment(post.Id);
                            eventComentariu();
                            $("#commentGetter" + post.Id).click(eventComentariu);
                            $("#like" + post.Id).click(eventLike);
                            $("#commentAdd" + post.Id).click(eventAddComment);
                            $("#postRemove" + post.Id).click(prevent);
                            $("#postRemove" + post.Id).click(eventDeletePost);
                           
                        }
                        if (result.length !== 0) { canGet = true; }
                    }


                })
            }

        }

    })(document.getElementById("postGetter").dataset.user);
    eventPost();
    let functionCopy = eventPost;
    eventPost = () => { }
    this.setTimeout(() => {
        eventPost = functionCopy;
    }, 1000);
    $(window).scroll(() => {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight-1500) {
            eventPost();
            eventPost = () => { }
            this.setTimeout(() => {
                eventPost = functionCopy;
            }, 1000)
        }

    })

});