using KFD.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Kitchen.Controllers
{
    [Area("Kitchen")]
    //[Authorize(Roles = Utilities.StaticValues.Role_Chef)]
    public class KitchenOrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public KitchenOrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
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
