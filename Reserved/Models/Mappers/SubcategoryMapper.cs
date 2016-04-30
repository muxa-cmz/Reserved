using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class SubcategoryMapper
    {
        private readonly DBMaster _dbMaster = new DBMaster();

        private List<Subcategory> GetSubcategory(String SQL)
        {
            _dbMaster.OpenConnection();
            List<Subcategory> subcategories = new List<Subcategory>();
            try
            {
                MySqlCommand command = _dbMaster.GetConnection().CreateCommand();
                command.CommandText = SQL;
                command.ExecuteNonQuery();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subcategories.Add(new Subcategory(Convert.ToInt32(reader["id"]),
                            reader["name"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _dbMaster.CloseConnection();
            return subcategories;
        }

        public List<Subcategory> GetSubcategory()
        {
            String SQL = "SELECT * FROM Subcategory";
            return GetSubcategory(SQL);
        }

    }
}