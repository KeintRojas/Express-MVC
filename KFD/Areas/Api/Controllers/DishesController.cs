using KFD.Data.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Api.Controllers
{
    [Area("Api")]
    public class DishesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DishesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region API
        public IActionResult GetAll()
        {
            var dishList = _unitOfWork.Dish.GetAll();
            return Json(new { data = dishList });
        }
        public IActionResult Details(int id)
        {
            var dish = _unitOfWork.Dish.Get(x => x.Id == id);
            if (dish == null)
            {
                return Json(new { success = false, message = "Error" });
            }
            return Json(new { data = dish });
        }
        #endregion
    }
}
