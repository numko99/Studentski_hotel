// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function PrikaziStudenta() {
    document.getElementById("ajaxTabela").style.display = "block";
    //var url = "/Home/LoginForm";
    var url = "/Identity/Account/Login";
    var zahtjev = new XMLHttpRequest();
    zahtjev.onload = function () {
        if (zahtjev.status === 200) {
            document.getElementById("ajaxTabela").innerHTML = zahtjev.responseText;
        } else {

        }
    };
    zahtjev.open("GET", url, true);
    zahtjev.send();

}
function ZatvoriDiv() {
    if (document.getElementById("ajaxTabela").style.display == "block") {

        document.getElementById("ajaxTabela").style.display = "none";
    }

}