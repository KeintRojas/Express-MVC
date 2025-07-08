using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace KFD.Areas.Kitchen.Controllers
{
    [Area ( "Kitchen" )]
    [Authorize ( Roles = Utilities.StaticValues.Role_Chef )]
    public class KitchenOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public KitchenOrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index ( )
        {
            return View ( );
        }

        #region Api

        public async Task<IActionResult> GetAll ( )
        {
            List<Order> orderList = new List<Order> ( );
            foreach ( var item in _unitOfWork.Order.GetAll ( ) )
            {
                if ( !item.State.Contains ( "Entregado" ) && !item.State.Contains ( "Anulado" ) )
                {
                    orderList.Add ( item );
                }
            }
            var result = new List<object>();

            foreach (var order in orderList)
            {
                var user = await _userManager.FindByIdAsync(order.CustomerId);
                result.Add(new
                {
                    order.Id,
                    order.Description,
                    order.Total,
                    order.Date,
                    order.State,
                    DeliveryBy = user?.Name ?? "Desconocido",
                    UserName = user?.UserName ?? "Desconocido"
                });
            }
            return Json ( new { data = result } );
        }

        [HttpPost]
        public IActionResult DeliverOrder ( int? id )
        {
            Order orderFromDb = _unitOfWork.Order.Get ( x => x.Id == id );
            if ( orderFromDb == null )
            {
                return Json ( new { success = false , message = "Error: El pedido no fue encontrado para entregar." } );
            }
            orderFromDb.State = "Entregado";
            _unitOfWork.Order.Update ( orderFromDb );
            _unitOfWork.Save ( );
            return Json ( new { success = true , message = "Pedido entregado correctamente." } ); // <-- Cambiado a success: true
        }

        [HttpPost]
        public IActionResult CancelOrder ( int? id )
        {
            Order orderFromDb = _unitOfWork.Order.Get ( x => x.Id == id );
            if ( orderFromDb == null )
            {
                return Json ( new { success = false , message = "Error: El pedido no fue encontrado para anular." } );
            }
            orderFromDb.State = "Anulado";
            _unitOfWork.Order.Update ( orderFromDb );
            _unitOfWork.Save ( );
            return Json ( new { success = true , message = "Pedido anulado correctamente." } ); // <-- Cambiado a success: true
        }

        #endregion
    }
}