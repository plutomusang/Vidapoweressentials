$(window).ready(function () {


});

$("#membership_submit").click(function () {
    var frmSerial = "&" + $("#membership_form").serialize();
    alert(frmSerial);
});

$("#loginid").click(function () {
    var sURL = getRootUrl();
    var win = window.open(sURL + '/UserAccount/');

});
function getRootUrl() {
    var defaultPorts = { "http:": 80, "https:": 443 };

    return window.location.protocol + "//" + window.location.hostname
               + (((window.location.port)
                && (window.location.port != defaultPorts[window.location.protocol]))
                ? (":" + window.location.port) : "");
}