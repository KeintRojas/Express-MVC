$(document).ready(function () {
    loadCards(); 

    
    setInterval(function () {
        loadCards();
    }, 10000); 
});


async function loadCards() {
    const cardsContainer = $('#cardsContainer');

    try {
        const response = await fetch("/Kitchen/KitchenOrders/GetAll");

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({ message: `Error HTTP! Estado: ${response.status}` }));
            throw new Error(errorData.message || `Error HTTP! Estado: ${response.status}`);
        }

        const responseData = await response.json();
        const orders = responseData.data;

        cardsContainer.empty();

        if (Array.isArray(orders) && orders.length > 0) {
            orders.forEach(item => {
                let statusBgClass = "";
                let statusTextColorClass = "";
                let buttonHtml = '';

                const orderDate = new Date(item.date);
                const formattedDate = orderDate.toLocaleDateString('es-CR', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: true
                });
                
                switch (item.state) {
                    case "A Tiempo":
                        statusBgClass = "bg-success";
                        statusTextColorClass = "text-white";
                        break;
                    case "Sobre Tiempo":
                        statusBgClass = "bg-warning";
                        statusTextColorClass = "text-dark";
                        break;
                    case "Demorado":
                        statusBgClass = "bg-danger";
                        statusTextColorClass = "text-white";
                        break;
                    case "Entregado":
                        statusBgClass = "bg-dark";
                        statusTextColorClass = "text-white";
                        buttonHtml = `<span class="badge bg-light text-dark">${item.state}</span>`;
                        break;
                    case "Anulado":
                        statusBgClass = "bg-danger";
                        statusTextColorClass = "text-white";
                        buttonHtml = `<span class="badge bg-light text-dark">${item.state}</span>`;
                        break;
                    default:
                        statusBgClass = "bg-info";
                        statusTextColorClass = "text-white";
                        break;
                }

                if (item.state !== "Entregado" && item.state !== "Anulado") {
 
                    buttonHtml = `
                        <button class="btn btn-sm btn-success update-status-btn me-2" data-id="${item.id}" data-action="DeliverOrder">Entregado</button>
                        <button class="btn btn-sm btn-danger update-status-btn" data-id="${item.id}" data-action="CancelOrder">Anular</button>
                    `;
                }

                const cardHtml = `
                    <div class="col-md-4 mb-4">
                        <div class="card h-100">
                            <div class="card-body">
                                <h5 class="card-title">De: ${item.deliveryBy}</h5>
                                <h5 class="card-text">${item.description}</h5>
                                <p class="card-text"><strong>Precio Total:</strong> $${item.total}</p>
                                <p class="card-text"><small class="text-muted">Fecha: ${formattedDate}</small></p>
                                <p class="card-text">
                                    <strong>Estado:</strong>
                                    <span class="badge ${statusBgClass} ${statusTextColorClass} py-2 px-3">
                                        ${item.state}
                                    </span>
                                </p>
                            </div>
                            <div class="card-footer d-flex justify-content-between align-items-center">
                                <div>
                                    ${buttonHtml}
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                cardsContainer.append(cardHtml);
            });
        } else {
            cardsContainer.append('<div class="col-12"><p class="alert alert-info">No hay pedidos para mostrar.</p></div>');
        }

    } catch (error) {
        console.error('Error al cargar los pedidos:', error);
        cardsContainer.empty();
        Swal.fire({
            icon: 'error',
            title: 'Error de Carga',
            text: `Hubo un error al cargar los pedidos: ${error.message || error}`,
            confirmButtonText: 'Ok'
        });
        cardsContainer.append('<div class="col-12"><p class="alert alert-danger">Hubo un error al cargar los pedidos. Por favor, revisa la consola para más detalles.</p></div>');
    }
}

/**
 * @param {number} orderId 
 * @param {string} action 
 */
async function performOrderAction(orderId, action) {
    let confirmText = '';
    let successText = '';
    let errorText = '';

    if (action === "DeliverOrder") {
        confirmText = `¿Deseas marcar el pedido ${orderId} como "Entregado"?`;
        successText = `Pedido #${orderId} marcado como "Entregado" con éxito.`;
        errorText = `No se pudo marcar el pedido ${orderId} como "Entregado".`;
    } else if (action === "CancelOrder") {
        confirmText = `¿Deseas marcar el pedido ${orderId} como "Anulado"? Esta acción no se puede deshacer.`;
        successText = `Pedido #${orderId} marcado como "Anulado" con éxito.`;
        errorText = `No se pudo marcar el pedido ${orderId} como "Anulado".`;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Acción Desconocida',
            text: 'La acción solicitada no es válida.',
            confirmButtonText: 'Ok'
        });
        return;
    }

    const result = await Swal.fire({
        title: `¿Estás seguro?`,
        text: confirmText,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `Sí, ${action === "DeliverOrder" ? 'Entregar' : 'Anular'}!`
    });

    if (!result.isConfirmed) {
        return;
    }

    try {
       
        const response = await fetch(`/Kitchen/KitchenOrders/${action}/${orderId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            
        });


        const responseData = await response.json();

        if (responseData.success) {
            Swal.fire({
                icon: 'success',
                title: '¡Operación Exitosa!',
                text: responseData.message || successText, 
                timer: 2000,
                timerProgressBar: true,
                showConfirmButton: false
            });
            loadCards(); 
        } else {
            throw new Error(responseData.message || errorText);
        }

    } catch (error) {
        console.error(`Error al ejecutar la acción '${action}' para el pedido ${orderId}:`, error);
        Swal.fire({
            icon: 'error',
            title: 'Error en la Operación',
            text: error.message || `Ocurrió un error inesperado al procesar el pedido ${orderId}.`,
            confirmButtonText: 'Ok'
        });
    }
}

$(document).on('click', '.update-status-btn', function () {
    const orderId = $(this).data('id');      
    const action = $(this).data('action');   
    performOrderAction(orderId, action);     
});