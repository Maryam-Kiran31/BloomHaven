using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class RosesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
