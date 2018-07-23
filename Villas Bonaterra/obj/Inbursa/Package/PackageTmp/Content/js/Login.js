

    var $body = $("body");

    $(document)
        .ajaxStart(function () {
            
                $body.addClass("loading");
            
        })
        .ajaxStop(function () {
           // setTimeout(function () {
                $body.removeClass("loading");
           // },1000);
    });

function RecoveryPassword() {
        
        var email = $("input#email").val();
 
        $.ajax({
            url: '/Login/Recovery/',
            type: 'POST',
            async: true,
            data: { email: email },
            success: function (data, status) {
                if (data.success) {
                    document.location.href = "/Login/RecoverySended/";
                } else {
                    $('#message').html(data.message);
                    $('#error').show();
                }
            },
            error: function (xhr, desc, err) {
                alert("Error Login.js");
                console.log(xhr);
                console.log("Details: " + desc + "\nError:" + err);
            }
        });
    }

    function RegisterUserClick() {

        var password = document.getElementById("InputPassword")
            , confirm_password = document.getElementById("ConfirmPassword");

        function validatePassword() {

            if (password.value != confirm_password.value) {
                confirm_password.setCustomValidity("Las contraseñas no son iguales");
            } else {
                confirm_password.setCustomValidity('');
            }
        }

        password.onchange = validatePassword;
        confirm_password.onkeyup = validatePassword;

        $("#submit").trigger("click");
    }

function LoginForm(e) {
    if ((e.keyCode == 13 && e.currentTarget.id == "password") || e.keyCode === undefined) {
        e.preventDefault();
        $body.addClass("loading");
        $("#LoginForm").submit();
    }
    }

    function ChangePasswordForm(e) {
        e.preventDefault();
        $body.addClass("loading");
        $("#LoginForm").submit();
    }
    $('#RegisterFrom').submit(function (e) {
        e.preventDefault();
        $('.btn.btn-primary.btn-block').append("<i class='fa fa-spinner fa-spin'></i>");
        var name = $('#InputName').val();
        var lastname = $('#InputLastName').val();
        var email = $('#InputEmail').val();
        var password = $('#InputPassword').val();


        $.ajax({
            url: '/Login/Register/',
            type: 'POST',
            data: { Name: name, LastName: lastname, Email: email, Password: password },
            success: function (data, status) {
                $('.fa.fa-spinner.fa-spin').remove();
                if (data.success) {
                    $('#typemessage').html("Aviso!");
                    $('#message').html(data.message);
                    $('#error').show();
                    setTimeout(function () {
                        document.location.href = "/Login/RegisterConfirmation/";
                    }, 2000);
                } else {
                    $('#typemessage').html("Error!");
                    $('#message').html(data.message);
                    $('#error').show();
                }
            },
            error: function (xhr, desc, err) {
                $('.fa.fa-spinner.fa-spin').remove();
                alert("Error Login.js");
                console.log(xhr);
                console.log("Details: " + desc + "\nError:" + err);
            }
        }); // end ajax call

    });

    function HideTextDanger() {
        $('.text-danger').hide();
    }

    function HideError() {
        $('#error').hide();
    }