using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Area.Controllers
{
    [Area("Area")]
    [Authorize(Roles = Utilities.StaticValues.Role_Admin)]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
                TempData["success"] = "Pedido editado correctamente";
                return RedirectToAction("Index");
            }
            TempData["error"] = "No se ha podido editar el pedido";

            return View();
        }
        #region Api
        public async Task<IActionResult> GetAll(DateTime? startDate, DateTime? endDate) 
        { 
            var orderList = await _unitOfWork.Order.GetAllAsync();
            if (startDate.HasValue)
                orderList = orderList.Where(o => o.Date >= startDate.Value).ToList();

            if (endDate.HasValue)
                orderList = orderList.Where(o => o.Date <= endDate.Value).ToList();

            var result = new List<object>();

            foreach (var order in orderList)
            {
                var user = await _userManager.FindByIdAsync(order.CustomerId);
                result.Add(new
                {
                    order.Id,
                    order.CustomerId,
                    order.Description,
                    order.Total,
                    order.Date,
                    order.State,
                    DeliveryBy = user?.Name ?? "Desconocido",
                    UserName = user?.UserName ?? "Desconocido"
                });
            }
            return Json(new { data = result });
        }
        #endregion
    }
}
