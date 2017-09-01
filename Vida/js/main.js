$(window).ready(function () {

    $('#msglws')
        .on('DataArival_Local',
            function (e, results) {
                
            }
        );
});

function apiPowercall(key, storedProcname, mform) {
        var form = "key=" + key + "&StoredProc=" + storedProcname + mform;
        $.ajax(
        {
            url: 'api/Powercall',
            type: "GET", 
            contentType: "json",
            data: form,
            success: function (data) {
                $('#msg').trigger('DataArival_Local', data);
            }
        });
}

function apiLogin(key, frmSerial) {
    var form = "key=" + key  + frmSerial;
    $.ajax(
    {
        url: 'api/Login3',
        type: "POST",
        contentType: "json",
        data: form,
        success: function (data) {
            $('#msg').trigger('DataArival_Local', data);
        }
    });


}

$("#membership_submit").click(function () {
    var frmSerial = "&" + $("#membership_form").serialize();
    alert(frmSerial);
});

$("#loginid").click(function () {
    //var sURL = getRootUrl();
    //var win = window.open(sURL + '/UserAccount/');
    var frmSerial = "&" + $("#LoginFormID").serialize();
    apiLogin('1234567890', frmSerial);
});
function getRootUrl() {
    var defaultPorts = { "http:": 80, "https:": 443 };

    return window.location.protocol + "//" + window.location.hostname
               + (((window.location.port)
                && (window.location.port != defaultPorts[window.location.protocol]))
                ? (":" + window.location.port) : "");
}