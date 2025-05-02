using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class BouquetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
