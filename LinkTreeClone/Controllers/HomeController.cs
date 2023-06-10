using LinkTreeClone.DB;
using Microsoft.AspNetCore.Mvc;

namespace LinkTreeClone.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Chat()
        {
            return View();
        }
    }
}
