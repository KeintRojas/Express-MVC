using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Area.Controllers
{
    [Area("Area")]
    [Authorize(Roles = Utilities.StaticValues.Role_Admin)]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Order orderFromDb = _unitOfWork.Order.Get(x=>x.Id==id);
            if (orderFromDb == null)
            {
                return NotFound();
            }

            return View(orderFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Order obj) {
            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            TempData["success"] = "Pedido editado correctamente";
            return View(obj);
        }
        #region Api
        public async Task<IActionResult> GetAll() 
        { 
            var orderList = await _unitOfWork.Order.GetAllAsync();
            return Json(new { data = orderList });
        }
        #endregion
    }
}
