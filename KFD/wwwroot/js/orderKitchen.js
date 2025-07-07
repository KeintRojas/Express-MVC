var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Kitchen/KitchenOrders/GetAll"
        },
        "columns": [
            { "data": "description", "width": "30%", "title": "Descripcion" },
            { "data": "total", "width": "10%", "title": "Precio Total" },
            { "data": "date", "width": "20%", "title": "Fecha" },
            { "data": "state", "width": "20%", "title": "Estado" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Area/Orders/Edit/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>Editar
                        </a>
                    `
                },
                "width": "20%"
            }

        ]
    });
}
