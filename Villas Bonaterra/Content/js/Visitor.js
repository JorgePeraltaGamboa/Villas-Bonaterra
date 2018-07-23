

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

$('#datepicker').datepicker({
    autoclose:true,
    format: "dd/mm/yyyy",
    startView:2,
    language: 'es'
});


$(document).on("click", ".nav-link", function () {
    var url = $(this).data('url');
    $(".modal-body #photo").attr("src", url);
    
});
/*
$("#Photo").change(function () {
    jQuery.each(jQuery('#Photo')[0].files, function (i, file) {
        $("#preview").attr("src", file);
        $("#preview").attr("display", "block");
    });    
});
*/

$('#VisitorForm').submit(function (e) {
    e.preventDefault();
    $('.btn.btn-primary.btn-block').append(" <i class='fa fa-spinner fa-spin'></i>");

    var name = $('#Name').val();
    var lastname = $('#LastName').val();
    var midlename = $('#MidleName').val();
    var birthdate = $('#BirthDate').val();
    
    var data = new FormData();
    jQuery.each(jQuery('#Photo')[0].files, function (i, file) {
        data.append('file-' + i, file);
    });

    data.append("Name", name);
    data.append("MidleName", midlename);
    data.append("LastName", lastname);
    data.append("BirthDay", birthdate);
    
    $.ajax({
        url: '/api/Visitor/Add',
        type: 'POST',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        //data: { Name: name, MidleName: midlename, LastName: lastname, BirthDay: birthdate, Photo: photo },
        
        success: function (data, status) {
            $('.fa.fa-spinner.fa-spin').remove();
            if (data.success) {
                $('#typemessage').html("Aviso!");
                $('#message').html(data.message);
                $('#error').show();
                setTimeout(function () {
                    document.location.href = "/Visitor/";
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

$('#VisitorUpdateForm').submit(function (e) {
    e.preventDefault();
    $('.btn.btn-primary.btn-block').append(" <i class='fa fa-spinner fa-spin'></i>");

    var id = $('#Id').val();
    var name = $('#Name').val();
    var lastname = $('#LastName').val();
    var midlename = $('#MidleName').val();
    var birthdate = $('#BirthDate').val();
    var checked = $('#verificado').val();

    var data = new FormData();
    jQuery.each(jQuery('#Photo')[0].files, function (i, file) {
        data.append('file-' + i, file);
    });

    data.append("Id", id);
    data.append("Name", name);
    data.append("MidleName", midlename);
    data.append("LastName", lastname);
    data.append("BirthDay", birthdate);
    data.append("Verificado", checked);

    $.ajax({
        url: '/api/Visitor/Update',
        type: 'POST',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        //data: { Name: name, MidleName: midlename, LastName: lastname, BirthDay: birthdate, Photo: photo },

        success: function (data, status) {
            $('.fa.fa-spinner.fa-spin').remove();
            if (data.success) {
                $('#typemessage').html("Aviso!");
                $('#message').html(data.message);
                $('#error').show();
                setTimeout(function () {
                    document.location.href = "/Visitor/";
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

function getBase64(file) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        console.log(reader.result);
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#preview')
                .attr('src', e.target.result)
                .width(150)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#preview').attr('src', e.target.result);
            $('#previewform').css('display','block');
        }

        reader.readAsDataURL(input.files[0]);
    }
}

