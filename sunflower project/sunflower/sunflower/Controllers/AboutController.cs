using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
