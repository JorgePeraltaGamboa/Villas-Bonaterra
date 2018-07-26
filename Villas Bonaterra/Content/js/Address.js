$('#AddNewAddress').submit(function (e) {
    e.preventDefault();
    $('.btn.btn-primary.btn-block').append(" <i class='fa fa-spinner fa-spin'></i>");
    var street = $('#Street').val();
    var no_calle = $('#No_Calle').val();
    var ownerid = $('#OwnerId').val();
    $.ajax({
        url: '/api/Residence/Add',
        type: 'POST',
        data: { Street: street, No_Calle: no_calle, OwnerId: ownerid },
        cache: false,
        contentType: 'application/x-www-form-urlencoded',
        success: function (data, status) {
            $('.fa.fa-spinner.fa-spin').remove();
            if (data.success) {
                $('#typemessage').html("Aviso!");
                $('#message').html(data.message);
                $('#error').show();
                setTimeout(function () {
                    document.location.href = "/Address/?id=" + ownerid;
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

$('.Disassociate').click(function (e) {
    var value = $(this).data('id');
    $('#exampleModal').modal('show');
});