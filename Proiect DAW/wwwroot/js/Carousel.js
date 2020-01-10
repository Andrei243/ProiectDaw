
window.addEventListener("load", () => {
    $(".poze").each((index, div) => {
        
        $.ajax({
        type:'GET',
        url: '/Profile/GetPhotos',
        data: {
            albumId: div.dataset.id
        },
        success: function (response) {
            if(response.length>0){
                let photosHtml = "";
                for (let i = 0; i < response.length; i++) {
                    let beginAnchor = `<a href="/Photos/Download/${response[i].Id}" data-lightbox="album${div.dataset.id}" data-title="${response[i].Description ? response[i].Description : ""}" data-alt="${response[i].Description ? response[i].Description : ""}" `;
                    if (i === 0) {
                        beginAnchor = beginAnchor + ">";
                    }
                    else {
                        beginAnchor = beginAnchor + " hidden>";
                    }
                    let photo = `<img class="albumPreview" src="/Photos/Download/${response[i].Id}"/>`
                    let finishAnchor = "</a>";
                    photosHtml = photosHtml + '\n' + beginAnchor + '\n' + photo + '\n' + finishAnchor;
                }
                div.innerHTML = photosHtml;
            }
        }

    })

    })
    lightbox.option({
        'disableScrolling': true,
        'maxHeight': Math.floor(window.innerHeight * 0.7),
        'maxWidth': Math.floor(window.innerWidth * 0.7),
        'wrapAround': true
    })
})