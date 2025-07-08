using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
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

        public KitchenOrdersController ( IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index ( )
        {
            return View ( );
        }

        public IActionResult Edit ( int? id )
        {
            if ( id == null || id == 0 )
            {
                return NotFound ( );
            }
            Order orderFromDb = _unitOfWork.Order.Get ( x => x.Id == id );
            if ( orderFromDb == null )
            {
                return NotFound ( );
            }

            return View ( orderFromDb );
        }

        [HttpPost]
        public IActionResult Edit ( Order obj )
        {
            if ( ModelState.IsValid )
            {
                _unitOfWork.Order.Update ( obj );
                _unitOfWork.Save ( );
                TempData [ "success" ] = "Pedido editado correctamente";
                return RedirectToAction ( "Index" );
            }
            return View ( obj );
        }

        #region Api

        public IActionResult GetAll ( )
        {
            List<Order> orderList = new List<Order> ( );
            foreach ( var item in _unitOfWork.Order.GetAll ( ) )
            {
                if ( !item.State.Contains ( "Entregado" ) && !item.State.Contains ( "Anulado" ) )
                {
                    orderList.Add ( item );
                }
            }
            return Json ( new { data = orderList } );
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
            return Json ( new { success = true , message = "Pedido entregado correctamente." } ); 
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
            return Json ( new { success = true , message = "Pedido anulado correctamente." } ); 
        }

        #endregion
    }
}