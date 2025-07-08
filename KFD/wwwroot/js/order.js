var dataTable;

$(document).ready(function () {
    loadDataTable();
    setInterval(function () {
        dataTable.ajax.reload(null, false); 
    }, 10000);
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Area/Orders/GetAll"
        },
        "columns": [
            { "data": "description", "width": "30%", "title": "Descripcion" },
            { "data": "total", "width": "10%", "title": "Precio Total" },
            { "data": "date", "width": "20%", "title": "Fecha" },
            {
                "data": "state",
                "width": "20%",
                "title": "Estado",
                "render": function (data) {
                    let cardClass = "";
                    let text = data;
                    switch (data) {
                        case "A Tiempo":
                            cardClass = "bg-success text-dark";
                            break;
                        case "Sobre Tiempo":
                            cardClass = "bg-warning text-dark";
                            break;
                        case "Demorado":
                            cardClass = "bg-danger text-dark";
                            break;
                        case "Anulado":
                            cardClass = "bg-danger text-dark";
                            break;
                        default:
                            cardClass = "bg-secondary text-dark";
                            break;
                    }
                    return `
                        <div class="card ${cardClass}" style="margin:0; padding:0;">
                            <div class="card-body p-2 text-center" style="font-weight:bold;">
                                ${text}
                            </div>
                        </div>
                    `;
                }
            },
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
