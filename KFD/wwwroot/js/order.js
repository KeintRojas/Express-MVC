var dataTable;

var dataTable;

$(document).ready(function () {
    if (localStorage.getItem('orderFilterStartDate')) {
        $('#startDate').val(localStorage.getItem('orderFilterStartDate'));
    }
    if (localStorage.getItem('orderFilterEndDate')) {
        $('#endDate').val(localStorage.getItem('orderFilterEndDate'));
    }

    loadDataTable();

    setInterval(function () {
        dataTable.ajax.reload(null, false);
    }, 30000);

    $('#filterBtn').click(function () {
        const start = $('#startDate').val();
        const end = $('#endDate').val();

        localStorage.setItem('orderFilterStartDate', start);
        localStorage.setItem('orderFilterEndDate', end);

        dataTable.ajax.reload();
    });

    $('#resetBtn').click(function () {
        $('#startDate').val('');
        $('#endDate').val('');

        localStorage.removeItem('orderFilterStartDate');
        localStorage.removeItem('orderFilterEndDate');

        dataTable.ajax.reload();
    });

    $('#startDate, #endDate').on('keypress', function (e) {
        if (e.which === 13) {
            $('#filterBtn').click();
        }
    });
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            url: "/Area/Orders/GetAll",
            data: function (d) {
                const start = $('#startDate').val();
                const end = $('#endDate').val();
                if (start) d.startDate = start;
                if (end) d.endDate = end;
            }
        },
        columns: [
            { data: "deliveryBy", width: "20%", title: "Cliente" },
            { data: "userName", width: "30%", title: "Usuario" },
            { data: "description", width: "30%", title: "Descripcion" },
            { data: "total", width: "10%", title: "Precio Total" },
            { data: "date", width: "20%", title: "Fecha" },
            {
                data: "state",
                width: "20%",
                title: "Estado",
                render: function (data) {
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
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Area/Orders/Edit/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>Editar
                        </a>
                    `;
                },
                width: "20%"
            }
        ]
    });
}

