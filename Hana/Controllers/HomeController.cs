using Hana.DB;
using Microsoft.AspNetCore.Mvc;

namespace Hana.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BootTheme()
        {
            return View();
        }


        public IActionResult Chat()
        {
            return View();
        }
    }
}
