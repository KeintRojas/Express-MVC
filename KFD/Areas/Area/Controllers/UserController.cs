using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KFD.Areas.Area.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<User> userList = unitOfWork.User.GetAll().ToList();
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
                unitOfWork.User.Add(obj);
                unitOfWork.Save();
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
            User userFromDB = unitOfWork.User.Get(x => x.Id == id);
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
                unitOfWork.User.Update(obj);
                unitOfWork.Save();
                TempData["success"] = "User Edited Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
