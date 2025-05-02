
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sunflower.Models;

namespace Application.Services
{
    public class CategoryService
    {
        //private readonly Addproduct<Category> _addUseCase;
        private readonly IRepository<Category> _repository;
        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public void AddCategory(Category category)
        {
            _repository.Add(category);
        }

        //public IEnumerable<Category> GetAllCategories()
        //{
        //    return _categoryRepository.GetAllC();
        //}

        //public Category GetCategoryById(int id)
        //{
        //    return _categoryRepository.GetbyID(id).FirstOrDefault();
        //}
 

        //public void UpdateCategory(Category category)
        //{
        //    _categoryRepository.Update(category);
        //}

        //public void DeleteCategory(string name)
        //{
        //    _categoryRepository.Delete(name);
        //}

        //public Category FindCategoryByName(string name)
        //{
        //    return _categoryRepository.FindByName(name);
        //}
    }
}
