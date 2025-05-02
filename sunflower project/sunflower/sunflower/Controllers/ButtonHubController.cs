using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class ButtonHubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
