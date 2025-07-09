$(document).ready(function () {
    loadCards();

    setInterval(function () {
        loadCards();
    }, 10000);
});

async function loadCards() {
    const cardsContainer = $('#cardsContainer');
    const maxOrdersToShow = 10; // Define el número máximo de pedidos a mostrar

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
            const ordersToShow = orders.slice(0, maxOrdersToShow); 
            const hasMoreOrders = orders.length > maxOrdersToShow; 

            ordersToShow.forEach(item => {
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
                        <button class="btn btn-sm btn-success update-status-btn me-2" data-id="${item.id}"
                        data-action="DeliverOrder"
                        data-state="${item.state}">Entregado</button>
                        <button class="btn btn-sm btn-danger update-status-btn" data-id="${item.id}"
                        data-action="CancelOrder"
                        data-state="${item.state}">Anular</button>
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

           
            if (hasMoreOrders) {
                cardsContainer.append(`
                    <div class="col-12 mt-3">
                        <p class="alert alert-info text-center">Hay ${orders.length - maxOrdersToShow} pedido(s) más en espera.</p>
                    </div>
                `);
            }

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
 * Función asíncrona para enviar una acción específica (entregar/cancelar) a un pedido en el servidor.
 * Muestra SweetAlert2 para confirmación y notificaciones de éxito/error.
 *
 * @param {number} orderId - El ID del pedido a afectar.
 * @param {string} action - La acción a realizar ("DeliverOrder" o "CancelOrder").
 */
async function performOrderAction(orderId, action, currentState) {
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

        if (action === "CancelOrder" || action === "DeliverOrder") {
            localStorage.setItem("orderId", orderId)
            localStorage.setItem("previousState", currentState)
        }
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
    const state = $(this).data('state');
    performOrderAction(orderId, action, state);
});

$("#btnUndo").click(async function () {
    const orderId = localStorage.getItem("orderId");
    const previousState = localStorage.getItem("previousState");

    if (!orderId || !previousState) {
        Swal.fire({
            icon: 'error',
            title: 'Sin Información',
            text: 'No hay un pedido para deshacer!',
            confirmButtonText: 'Ok',
            cancelButtonText: 'Cancelar'
        });
        return;
    }

    const result = await Swal.fire({
        title: '¿Quieres deshacer la anulación?',
        text: `¿Deseas deshacer el último cambio del pedido #${orderId}?`,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, deshacer!',
        cancelButtonText: 'No, cancelar'
    });

    if (!result.isConfirmed) {
        return;
    }

    try {
        const response = await fetch(`/Kitchen/KitchenOrders/UndoOrder/${orderId}?previousState=${encodeURIComponent(previousState)}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
            }
        });

        if (response.ok) {
            Swal.fire({
                title: 'Pedido restaurado exitosamente!',
                text: 'El pedido se devolvió a su estado anterior',
                icon: 'success',
                timer: 2000,
                timerProgressBar: true,
                showConfirmButton: false,
                showCancelButton: true
            });

            localStorage.removeItem("orderId");
            localStorage.removeItem("previousState");

            loadCards();
        } else {
            throw new Error(`Error al restaurar el pedido: ${response.statusText}`);
        }
    } catch (error) {
        console.error('Error al deshacer la anulación:', error);
        Swal.fire({
            icon: 'error',
            title: 'Error al Deshacer',
            text: `No se pudo deshacer la anulación del pedido #${orderId}: ${error.message}`,
            confirmButtonText: 'Ok'
        });
    }
});