window.addEventListener("load", () => {

    inaltimeTotala = window.innerHeight;
    inaltimeHeader = $("header").height();
    inaltimeFooter = $("footer").height();
    inaltimaMaximaPoza = inaltimeTotala - inaltimeHeader - inaltimeFooter - 100;
    $("#imagineLogin").height(inaltimaMaximaPoza);
})