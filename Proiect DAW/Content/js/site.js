// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var prevent = (e) => {
    var x = confirm("Are you sure of your action?");
    if (!x) {
        e.preventDefault();
        e.stopImmediatePropagation();
    }
}
window.addEventListener("load", () => {
    $('a.needConfirmation,input.needConfirmation').on("click", prevent);

})

