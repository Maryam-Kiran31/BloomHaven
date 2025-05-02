using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class JeweleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
