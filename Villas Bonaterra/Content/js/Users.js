$('#UsersUpdateForm').submit(function (e) {
    e.preventDefault();
    $('.btn.btn-primary.btn-block').append(" <i class='fa fa-spinner fa-spin'></i>");
    var id = $('#Id').val();
    var name = $('#Name').val();
    var lastname = $('#LastName').val();
    var midlename = $('#MidleName').val();
    var correo = $('#Correo').val();
    var telefono = $('#Telefono').val();
    //alert(id + name + lastname + midlename + correo + telefono);
    $.ajax({
        url: '/api/User/Update',
        type: 'POST',
        data: { Id: id, Name: name, MidleName: midlename, LastName: lastname, Correo: correo, Telefono: telefono },
        cache: false,
        contentType: 'application/x-www-form-urlencoded',

        //data: { Name: name, MidleName: midlename, LastName: lastname, Correo: correo, Telefono: telefono },

        success: function (data, status) {
            $('.fa.fa-spinner.fa-spin').remove();
            if (data.success) {
                $('#typemessage').html("Aviso!");
                $('#message').html(data.message);
                $('#error').show();
                setTimeout(function () {
                    document.location.href = "/Users/";
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
    });
});

$('#UsersFormAdd').submit(function (e) {
    e.preventDefault();
    $('.btn.btn-primary.btn-block').append(" <i class='fa fa-spinner fa-spin'></i>");

    var name = $('#Name').val();
    var lastname = $('#LastName').val();
    var midlename = $('#MidleName').val();
    var correo = $('#Correo').val();
    var telefono = $('#Telefono').val();


    $.ajax({
        url: '/api/User/Add',
        type: 'POST',
        data: { Name: name, MidleName: midlename, LastName: lastname, Correo: correo, Telefono: telefono },
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        
        //data: { Name: name, MidleName: midlename, LastName: lastname, Correo: correo, Telefono: telefono },

        success: function (data, status) {
            $('.fa.fa-spinner.fa-spin').remove();
            if (data.success) {
                $('#typemessage').html("Aviso!");
                $('#message').html(data.message);
                $('#error').show();
                setTimeout(function () {
                    document.location.href = "/Users/";
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
    });
});
