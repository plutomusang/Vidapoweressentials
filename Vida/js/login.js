$(function () {

    //-- validate if email
    $('#email').blur(function () {
        if (!isEmail($(this).val())) {
            $(this).next().text('Invalid email address');
            $(this).parent().addClass('has-error');
        } else {
            $(this).next().text('');
            $(this).parent().removeClass('has-error');
        }
    });

    $('#frmlogin').submit(function (event) {
        event.preventDefault();
        submitLogin();
    });
});

function submitLogin() {

    $('#loginid').prop('disabled', true);
    $('#iloader').show();
    $('#password').next().text('');
    $('#password').parent().removeClass('has-error');


    $.post(baseUrl() + "api/Login", { 'Username': $('#email').val(), 'Password': $('#password').val(), 'key': generalKey() }, function (data) {
        // console.log(data);
        $('#password').val('');
        if (data.ValidateUser != '' && data.key != undefined && data.key != '') {
            var buildParam = '?appkey=' + data.key + "&email=" + $('#email').val()+ "&uid=" + data.ValidateUser[0].UserID+"&tok=" + 
                    data.ValidateUser[0].Code + '&ReturnUrl=' + getUrlParameter('ReturnUrl');
            window.location.replace(baseUrl() + data.url + buildParam);
        } else {
            $('#iloader').hide();
            $('#loginid').prop('disabled', false);
            $('#password').next().text('Sorry, something is not right.');
            $('#password').parent().addClass('has-error');
        }


    }, 'json');
}