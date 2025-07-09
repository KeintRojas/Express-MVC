using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Area.Controllers
{
    [Area("Area")]
    [Authorize(Roles = Utilities.StaticValues.Role_Admin)]
    public class DishesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public DishesController(IUnitOfWork unitOfWork, 
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Dish obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    var uploads = Path.Combine(wwwRootPath, @"images/dishes");
                    if (obj.Picture != null)
                    {
                        var oldImageURL = Path.Combine(wwwRootPath, obj.Picture);
                        if (oldImageURL != Path.Combine(uploads, Utilities.StaticValues.Image_Unavailable))
                        {
                            if (System.IO.File.Exists(oldImageURL))
                            {
                                System.IO.File.Delete(oldImageURL);
                            }
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads
                        , fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Picture = @"/images/dishes/" + fileName + extension;
                }
                else { 
                    obj.Picture = @"/images/dishes/" + Utilities.StaticValues.Image_Unavailable;
                }
                
                _unitOfWork.Dish.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Plato Guardado Correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Dish dishFromDB = _unitOfWork.Dish.Get(x => x.Id == id);
            if (dishFromDB == null)
            {
                return NotFound();
            }
            return View(dishFromDB);
        }
        [HttpPost]
        public IActionResult Edit(Dish obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    var uploads = Path.Combine(wwwRootPath, @"images/dishes");
                    if (obj.Picture != null)
                    {
                        var oldImageURL = obj.Picture;
                        if (oldImageURL != Path.Combine(uploads, Utilities.StaticValues.Image_Unavailable))
                        {
                            if (System.IO.File.Exists(oldImageURL))
                            {
                                System.IO.File.Delete(oldImageURL);
                            }
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads
                        , fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Picture = @"/images/dishes/" + fileName + extension;
                }
               
                _unitOfWork.Dish.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Plato Editado Correctamente";
                return RedirectToAction("Index");
            }
            return View();
        }

        #region API
        public IActionResult GetAll()
        {
            var dishList = _unitOfWork.Dish.GetAll();
            return Json(new { data = dishList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var dishToDelete = _unitOfWork.Dish.Get(x => x.Id == id);
            if (dishToDelete == null)
            {
                return Json(new { success = false, message = "Error al Eliminar" });
            }
            _unitOfWork.Dish.Remove(dishToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Plato Eliminado Correctamente" });
        }
        #endregion
    }

}
