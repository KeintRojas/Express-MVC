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
            { "data": "userName", "width": "20%", "title": "Nombre de Usuario" },
            { "data": "email", "width": "30%", "title": "Correo Electronico" },
            {
                "data": "isEnabled", "width": "10%", "title": "Estado", "render": function (data) {
                    return data === 1 ? "Activo" : "Desactivado";
                } },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Area/User/Edit/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>Editar
                        </a>
                        <a onClick=Delete('${data}') class="btn btn-danger mx-2">
                            <i class="bi bi-x-circle"></i>Eliminar
                        </a>
                    `
                },
                "width": "20%"
            }
        ]
    });
}
function Delete(_id) {

    Swal.fire({
        title: "Estas seguro?",
        text: "No podras revertir el cambio!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, Borrar!",
        cancelButtonText: "No, me arrepiento!",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Area/User/Delete/" + _id,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Error connecting to endpoint");
                }
            });
        }
    });
}