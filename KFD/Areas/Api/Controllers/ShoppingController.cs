using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Api.Controllers
{
    [Area("Api")]
    public class ShoppingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        #region API
        [HttpPost]
        public IActionResult Create([FromBody] Order obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Shopping Save Successfully";
                return Ok(new { message = "Pedido guardado exitosamente." });
            }
            return BadRequest(ModelState);
        }
        #endregion
    }
}
