using Microsoft.Data.SqlClient;
using Dapper;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Data;

namespace sunflower.Models
{ 
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        string connstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sunflower;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public GenericRepository()
        {
            Trace.WriteLine("GenericRepository instantiated.");
        }
        public IEnumerable<TEntity> GetAll()
        {
            var tableName = typeof(TEntity).Name;
            var query = $"SELECT * FROM {tableName}";

            Trace.WriteLine($"GetAll method called. Query: {query}");

            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    var result = connection.Query<TEntity>(query).ToList();
                    Trace.WriteLine($"Retrieved {result.Count} entities.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in GetAll method: {ex.Message}");
                throw;
            }
        }
        public void Add(TEntity entity)
            {
                var tablename = typeof(TEntity).Name;
                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");
                var columnNames = string.Join(",", properties.Select(x => x.Name));
                var par = string.Join(",", properties.Select(y => "@" + y.Name));
                var query = $"INSERT INTO {tablename} ({columnNames}) VALUES ({par})";

                Trace.WriteLine($"Add method called. Query: {query}");
                Debug.WriteLine($"Entity to be added: {entity}");

              //  try
                //{
                    using (var connection = new SqlConnection(connstring))
                    {
                        connection.Open();
                        connection.Execute(query, entity);
                       // Trace.WriteLine("Entity added successfully.");
                    }
                //}
                //catch (Exception ex)
                //{
                //    Trace.TraceError($"Error in Add method: {ex.Message}");
                //    throw;
                //}
            }
        //add category
        public void AddCata(Category c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c), "Category object cannot be null");
            }

            string n = c.Name;
            if (string.IsNullOrEmpty(n))
            {
                throw new ArgumentException("Category name cannot be null or empty", nameof(c));
            }

            string query = "INSERT INTO Catagory (Name) VALUES (@Name)";

            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@Name";
                    param.SqlDbType = SqlDbType.NVarChar;
                    param.Size = 100; // Adjust size based on your database schema
                    param.Value = n;

                    cmd.Parameters.Add(param);

                    // Log the command text and parameters for debugging purposes
                    Console.WriteLine("Command Text: " + cmd.CommandText);
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        Console.WriteLine($"Parameter Name: {p.ParameterName}, Value: {p.Value}");
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        } 
        public IEnumerable<TEntity> GetbyID(int id)
        {
            var tableName = typeof(TEntity).Name;
            var query = $"SELECT * FROM {tableName} WHERE catId = {id}";
            var result = new List<TEntity>();

            Trace.WriteLine($"GetbyID method called. Query: {query}, Id: {id}");

            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    var cmd = new SqlCommand(query, connection);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        while (reader.Read())
                        {
                            var entity = new TEntity();
                            foreach (var prop in properties)
                            {
                                if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                                {
                                    var value = reader.GetValue(reader.GetOrdinal(prop.Name));
                                    prop.SetValue(entity, value);
                                }
                            }
                            result.Add(entity);
                        }
                    }
                }
                Trace.WriteLine($"Retrieved {result.Count} entities with Id: {id}.");
                return result;
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in GetbyID method: {ex.Message}");
                throw;
            }
        
        }

        public void Delete(string Name)
        {
            var tableName = typeof(TEntity).Name;
            var query = $"DELETE FROM {tableName} WHERE Name = @Name";

            Trace.WriteLine($"Delete method called. Query: {query}, Name: {Name}");

            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    connection.Execute(query, new { Name = Name });
                    Trace.WriteLine("Entity deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in Delete method: {ex.Message}");
                throw;
            }
        }
       
        public void Update(TEntity entity)
        {
            var tableName = typeof(TEntity).Name;
            var primaryKey = "Name";
            var properties = typeof(TEntity).GetProperties()
                               .Where(x => x.Name != primaryKey && x.Name != "Id"); // Exclude Id and primaryKey (Name)

            var setClause = string.Join(",", properties.Select(a => $"{a.Name}=@{a.Name}"));

            var query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @{primaryKey}";

            Trace.WriteLine($"Update method called. Query: {query}");
            Debug.WriteLine($"Entity to be updated: {entity}");

            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    connection.Execute(query, entity);
                    Trace.WriteLine("Entity updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in Update method: {ex.Message}");
                throw;
            }
        }

        public TEntity FindByName(string name)
        {
            var tableName = typeof(TEntity).Name;
            var query = $"SELECT * FROM [{tableName}] WHERE Name = @name";

            Trace.WriteLine($"FindByName method called. Query: {query}, Name: {name}");

            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    var result = connection.QuerySingleOrDefault<TEntity>(query, new { name });
                    Trace.WriteLine(result != null ? "Entity found." : "Entity not found.");
                    return result;
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Error in FindByName method: {ex.Message}");
                throw;
            }
        }
    }


}
