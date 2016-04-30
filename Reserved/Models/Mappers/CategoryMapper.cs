using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class CategoryMapper
    {
        private readonly DBMaster _dbMaster = new DBMaster();

        private List<Category> GetCategory(String SQL)
        {
            _dbMaster.OpenConnection();
            List<Category> categories = new List<Category>();
            try
            {
                MySqlCommand command = _dbMaster.GetConnection().CreateCommand();
                command.CommandText = SQL;
                command.ExecuteNonQuery();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category(Convert.ToInt32(reader["id"]),
                            reader["name"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _dbMaster.CloseConnection();
            return categories;
        }

        public List<Category> GetCategory()
        {
            String SQL = "SELECT * FROM Subcategory";
            return GetCategory(SQL);
        }

    }
}