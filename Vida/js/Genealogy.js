$(window).ready(function () {

    var key = "29795F66-BBAD-41B3-8212-F233D0B02CC5";
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
