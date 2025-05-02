
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;

using sunflower.Data;
using sunflower.Models;
using System.Diagnostics;
using Dapper;
using Microsoft.EntityFrameworkCore; // This is required for ToListAsync()
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using sunflower.NotiHubs;


namespace sunflower.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHubContext<NotifyHubs> _hubContext;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Products> _p;
        private readonly IRepository<Category> _n;

        public ProductController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context,
            IRepository<Products> productRepository, IRepository<Category> categoryRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _p = productRepository;
            _n = categoryRepository;
        }

        // Admin-specific product display
        public async Task<IActionResult> AdminDisplay()
        {
            // Fetch all products or modify as needed for admin view
            var products = await _context.Products.ToListAsync();
            return View(products); // This view can contain admin-specific functionality (add, update, delete)
        }
        [Authorize(Policy="AdminAccessPolicy")]
        public ViewResult AddProduct()
        {
            return View();
        } 

        public ViewResult Wedding(int id)
        {
            List<Products> products = _p.GetbyID(id).ToList();
            return View(products);
          
        } 
        public async Task<IActionResult> Upload(Addproduct model, string Name, int Price, int Quantity, string Category)
        {
           
            if (ModelState.IsValid)
            {
                Products product = new Products { Name = Name, Price = Price, Quantity = Quantity };

                var uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "uploadedFiles");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string con = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sunflower;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

                using (SqlConnection projectConnection = new SqlConnection(con))
                {
                    projectConnection.Open();
                    string categoryIdQuery = $"SELECT id FROM category WHERE name ='{Category}'";
                    SqlCommand categoryIdCommand = new SqlCommand(categoryIdQuery, projectConnection);
                    object result = categoryIdCommand.ExecuteScalar();
                    int categoryId = Convert.ToInt32(result);

                    product.Catid = categoryId;
                }

                if (model.PathOfImage != null)
                {
                    string filePath = Path.Combine(uploadDir, model.PathOfImage.FileName);
                    product.PathOfImage = Path.GetFileName(model.PathOfImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PathOfImage.CopyToAsync(stream);
                    }
                }

                _p.Add(product);


                //// Notify all clients (including admin) about the new product
                //await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"A new product has been added!");

                //// Save the product to the database
                //await _context.SaveChangesAsync();

                return Json(new { success = true }); 
            }

            // Return a partial view containing updated product list or other content
            return Json(new { success = false, message = "Failed to save the product." });
        }

        [Authorize(Policy = "AdminAccessPolicy")]
        public IActionResult Admin()
        {

            // IRepository<Products> rep = new GenericRepository<Products>();

            List<Products> products = _p.GetAll().ToList();
            return View(products);
        }
        [Authorize(Policy = "AdminAccessPolicy")]
        public ViewResult updateproduct()
        {

            return View();
        }
        //  [Authorize(Policy = "Category1")]
        [Authorize(Policy = "AdminAccessPolicy")]
        public ViewResult Deleteproduct()
        {
            return View();
        }
        // [Authorize(Policy = "Category1")]
        [Authorize(Policy = "AdminAccessPolicy")]
        //public async Task<IActionResult> Up(string name ,IFormFile image,int qunatity, int price)
        //{
        //    // IRepository<Products> rep = new GenericRepository<Products>();
        //    Products p = new() {Name = name, Quantity = qunatity, Price = price };
        //    p.PathOfImage = Path.GetFileName(image.FileName);
        //   // p.Catid = 2;
        //    _p.Update(p);
        //    return RedirectToAction("updateproduct", "Product");
        //}
        [HttpPost]
        public async Task<IActionResult> Up(string Name, IFormFile image, int Quantity, int Price)
        {
            // Fetch the existing product by name using the new method
            var product = _p.FindByName(Name); // Fetch from the database

            if (product == null)
            {
                // Handle case when the product with the given name doesn't exist
                return NotFound("Product not found");
            }

            // Update the fields
            Products p = new() { Quantity = Quantity, Price = Price  };

            // Only update PathOfImage if an image is provided
            if (image != null)
            {
                product.PathOfImage = Path.GetFileName(image.FileName);
            }

            // Call your update method to save changes to the database
            _p.Update(product);

            return RedirectToAction("updateproduct", "Product");
        }

        [Authorize(Policy = "AdminAccessPolicy")]
        [HttpPost]
        public IActionResult Del(string Name)
        {
            try
            {
                _p.Delete(Name);
                return Json(new { success = true, message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting product: " + ex.Message });
            }
        }
    }
} 
