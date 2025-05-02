using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace sunflower.Models
{
    public interface IRepository<TEntity>
    {

       public void Add(TEntity entity);
        public void AddCata(Category c);
        public IEnumerable<TEntity> GetAll();
        public IEnumerable<TEntity> GetbyID(int id);
        public void Delete(string Name);
        public void Update(TEntity entity);
        public TEntity FindByName(string name);
    }

}
