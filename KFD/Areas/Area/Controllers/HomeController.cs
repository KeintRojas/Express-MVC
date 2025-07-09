using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFD.Areas.Area.Controllers
{
    [Authorize(Roles = Utilities.StaticValues.Role_Admin)]
    [Area("Area")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
