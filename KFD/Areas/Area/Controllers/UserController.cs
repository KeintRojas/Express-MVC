using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    }
}
