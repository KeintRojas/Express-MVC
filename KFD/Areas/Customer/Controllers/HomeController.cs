using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Dish> dishList = _unitOfWork.Dish.GetAll();
            return View(dishList);
        }
        #region API
        public IActionResult Details(int id) 
        { 
            var dish = _unitOfWork.Dish.Get(x => x.Id ==  id);
            if (dish == null) {
                return Json(new { success = false, message = "Error" });
            }
            return Json(new { data = dish});
        }
        #endregion
    }
}
