using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;

namespace KFD.Areas.Area.Controllers
{
    [Area("Area")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<User> userList = _unitOfWork.User.GetAll().ToList();
            return View(userList);
        }

        public IActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        public IActionResult Create(User obj) 
        {
            if (ModelState.IsValid) { 
                _unitOfWork.User.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "User Created Successfully";
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
            User userFromDB = _unitOfWork.User.Get(x => x.Id == id);
            if (userFromDB == null)
            {
                return NotFound();
            }
            return View(userFromDB);
        }
        [HttpPost]
        public IActionResult Edit(User obj) {
            if (ModelState.IsValid) 
            {
                _unitOfWork.User.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "User Edited Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult DeletePost(int? id) 
        {
            if(id == null || id == 0) 
            { 
                return NotFound();
            }
            User? userFromDB = _unitOfWork.User.Get(x => x.Id==id);
            if (userFromDB == null) 
            {
                return NotFound() ;
            }
            _unitOfWork.User.Remove(userFromDB);
            _unitOfWork.Save();
            TempData["success"] = "User Deleted Successfully";
            return RedirectToAction("Index");
        }
        #region API
        public IActionResult Delete(int id) 
        { 
            var userToDelete = _unitOfWork.User.Get(x => x.Id==id);
            if (userToDelete == null) {
                return Json(new{success = false, message = "Error Deleting User"});
            }
            _unitOfWork.User.Remove(userToDelete);
            _unitOfWork.Save();
            return Json(new { success = false, message = "User Deleted Successfully" });
        }
        #endregion
    }
}
