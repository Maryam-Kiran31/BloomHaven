using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using sunflower.Models;
using System.Diagnostics;
using System.Text.Json;

namespace sunflower.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<Category> _p;
        private readonly IRepository<Products> _r;

        public HomeController(ILogger<HomeController> logger, IRepository<Category> p)
        {
            _logger = logger;
            _p = p;
        }
        //[HttpGet]
        //public ActionResult SearchResults(string Name)
        //{
        //    if (string.IsNullOrEmpty(Name))
        //    {
        //        return View(new List<Products>()); // Return an empty list if no search term
        //    }

        //    IRepository<Products> rep = new GenericRepository<Products>();
        //    List<Products> products = rep.GetAll().Where(p => p.Name.Contains(Name)).ToList();

        //    return View(products); // Pass the search results to the view
        //}

        // Backend Controller in C# (ASP.NET)
        [HttpGet]
        public JsonResult SearchResults(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new List<Products>()); // Return an empty JSON array
            }

            IRepository<Products> rep = new GenericRepository<Products>();
            List<Products> products = rep.GetAll().Where(p => p.Name.Contains(Name)).ToList();

            return Json(products, new JsonSerializerOptions { PropertyNamingPolicy = null });  // Proper JSON serialization
        }



        public IActionResult Index()
        {
            List<Category> products = _p.GetAll().ToList();
            return View(products);
        }
        //[Authorize(Policy = "AdminAccessPolicy")]
        public ActionResult partial_view()
        {
            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
