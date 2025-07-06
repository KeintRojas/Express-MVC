using KFD.Data.Repository.Interfaces;
using KFD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;

namespace KFD.Areas.Area.Controllers
{
    [Area("Area")]
    //[Authorize (Roles = Utilities.StaticValues.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Name = model.Name;
            user.IsEnabled = model.IsEnabled;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["success"] = "Usuario editado correctamente";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        #region API
        [HttpGet]
        public IActionResult CheckAuth()
        {

            return Ok(new
            {
                Username = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated
            });
        }
        public IActionResult GetAll()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.Name,
                u.IsEnabled,
                u.LockoutEnabled
            }).ToList();

            return Json(new { data = users });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error al encontrar el usuario" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Customer")) {
                user.IsEnabled = 0;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    return Json(new { success = true, message = "Usuario deshabilitado" });
                }
                return Json(new { success = false, message = "Error al deshabilitar al usuario" });
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Usuario eliminado correctamente" });
                }

                return Json(new { success = false, message = "Error eliminando el usuario" });
            }
        }
        #endregion
    }
}