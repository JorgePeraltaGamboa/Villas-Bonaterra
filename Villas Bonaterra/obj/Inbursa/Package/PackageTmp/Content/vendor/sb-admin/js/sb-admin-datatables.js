// Call the dataTables jQuery plugin
$(document).ready(function () {
    $('#dataTableVisitor').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        },
        "order": [[2, "desc"]]
    });
    $('#dataTable').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        }

    });
});