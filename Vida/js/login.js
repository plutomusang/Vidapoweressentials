$(function () {

    //-- validate if email
    $('#frmlogin').submit(function (event) {
        event.preventDefault();
        submitLogin();
    });
});

function submitLogin() {
    $('#iloader').show();

    $.post(baseUrl() + "api/Login", { 'Username': $('#email').val(), 'Password': $('#password').val(), 'key': generalKey() }, function (data) {
        // console.log(data);
        $('#password').val('');
        if (data.ValidateUser != '' && data.key != undefined && data.key != '') {
            var buildParam = '?appkey=' + data.key + "&email=" + $('#email').val()+ "&uid=" + data.ValidateUser[0].GenealogyID+"&tok=" + 
                    data.ValidateUser[0].Code + '&ReturnUrl=' + getUrlParameter('ReturnUrl');
            window.location.replace(baseUrl() + data.url + buildParam);
        } else {
            $('#iloader').hide();
            $("#errorMsg").show().delay(5000).fadeOut();
        }


    }, 'json');
}
