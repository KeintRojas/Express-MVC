var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Kitchen/Dishes/GetAll"
        },
        "columns": [
            { "data": "name", "width": "20%", "title": "Nombre del Plato" },
            { "data": "description", "width": "30%", "title": "Descripcion" },
            { "data": "price", "width": "10%", "title": "Precio" },
            {
                "data": "isEnabled", "width": "10%", "title": "Estatus", "render": function (data) {
                    return data === 1 ? "Habilitado" : "Deshabilitado";
                }
            },
            {
                "data": "picture",
                "width": "10%",
                "title": "Imagen",
                "render": function (data) {

                    return `
                                <div class="card" style="width: 70px;">
                                    <img src="/${data}" class="card-img-top" alt="picture" style="max-width: 60px; max-height: 60px; margin: auto; padding-top: 5px;" />
                                </div>
                            `;

                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Area/Dishes/Edit/${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i>Editar
                        </a>
                        <a onClick=Delete(${data}) class="btn btn-danger mx-2">
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
                url: "/Area/Dishes/Delete/" + _id,
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