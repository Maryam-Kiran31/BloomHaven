using sunflower.Models;
using System.Collections.Generic;
 

namespace Application.Services
{
    public class ProductService
    {
        private readonly IRepository<Products> _repository;

        public ProductService(IRepository<Products> repository)
        {
            _repository = repository;
        }

        //public IEnumerable<Products> GetProducts()
        //{
        //    var query = "SELECT * FROM Products";
        //    return _repository.GetAll(query);
        //}

        // Example of adding a new product
        public void AddProduct(Products product)
        {
            var query = "INSERT INTO Products (Name, Description, Price, CategoryId) VALUES (@Name, @Description, @Price, @CategoryId)";
            _repository.Add(product);
        }
    }
}
