using sunflower.Models;
using Microsoft.AspNetCore.Mvc;

namespace Haven.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _p;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CategoryController(IRepository<Category> p, IWebHostEnvironment hostingEnvironment)
        {
            _p = p;
            _hostingEnvironment = hostingEnvironment;
        }

        string s = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sunflower;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(string Name, Addproduct model)
        {
            var uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploadedFiles");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            Category c = new Category
            {
                Name = Name
            };
            if (model.PathOfImage != null)
            {
                string filePath = Path.Combine(uploadDir, model.PathOfImage.FileName);
                c.PathOfImage = Path.GetFileName(model.PathOfImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PathOfImage.CopyToAsync(stream);
                }
            }
            _p.Add(c);
            return View("Add");
        }

        
    }
}
