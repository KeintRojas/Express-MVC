var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Area/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "rol", "width": "10%" },
            { "data": "email", "width": "30%" },
            { "data": "isEnabled", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Area/User/Edit/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>Edit
                        </a>
                    `
                },
                "width": "20%"
            }
        ]
    });
}