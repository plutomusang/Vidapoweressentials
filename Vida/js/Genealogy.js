$(window).ready(function () {

    var key = "ApiKey1234";
    var storedProcname = "Get_Genealogy";
    var form = "key=" + key + "&StoredProc=" + storedProcname + "&GenealogyID=123456";
    $.ajax(
    {
        url: 'api/Genealogy',
        type: "GET",
        contentType: "json",
        data: form,
        success: function (data) {

            var d = data[storedProcname];
        }
    });



});
