using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sunflower.Controllers
{
    public class ContactController : Controller
    {
        [Authorize(Policy = "AdminAccessPolicy")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
