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
        #region API
        [HttpPost]
        public IActionResult Create([FromBody] Order obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Shopping Save Successfully";
                return Ok(new { orderId = obj.Id });
            }
            return BadRequest(ModelState);
        }
        [HttpPut]
        public async Task<IActionResult> CancelOrderStatus(int? id)
        {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "ID invalido" });
            }
            Order orderFromDB =  await _unitOfWork.Order.GetAsync(x => x.Id == id);
            if (orderFromDB == null) {
                return BadRequest(new { message = "Pedido no encontrado" });
            }
            if (orderFromDB.State == "Anulado")
            {
                return BadRequest(new { message = "El pedido ya está anulado" });
            }
            orderFromDB.State = "Anulado";
            _unitOfWork.Order.Update(orderFromDB);
            _unitOfWork.Save();
            return Ok(new { message = "Pedido anulado exitosamente." });
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderStatus(int? id) {
            if (id == null || id == 0)
            {
                return BadRequest(new { message = "ID invalido" });
            }
            Order orderFromDB = await _unitOfWork.Order.GetAsync(x => x.Id == id);
            if (orderFromDB == null)
            {
                return BadRequest(new { message = "Pedido no encontrado" });
            }
            return Ok(new { message = $"{orderFromDB.State}" });
        }
        #endregion
    }
}
