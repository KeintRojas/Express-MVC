using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Area.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
