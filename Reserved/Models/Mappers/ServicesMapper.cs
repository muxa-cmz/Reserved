using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Reserved.Models.DomainModels;
using Reserved.Models.Masters;

namespace Reserved.Models.Mappers
{
    public class ServicesMapper
    {
        private readonly DBMaster _dbMaster = new DBMaster();

        private List<Service> GetServices(String SQL)
        {
            _dbMaster.OpenConnection();
            List<Service> services = new List<Service>();
            try
            {
                MySqlCommand command = _dbMaster.GetConnection().CreateCommand();
                command.CommandText = SQL;
                command.ExecuteNonQuery();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        services.Add(new Service(Convert.ToInt32(reader["id"]),
                            reader["name"].ToString(),
                            reader["price"].ToString(),
                            reader["notation"].ToString(),
                            reader["duration"].ToString(),
                            reader["path_to_image"].ToString(),
                            Convert.ToInt32(reader["id_category"]),
                            Convert.ToInt32(reader["id_subcategory"])));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _dbMaster.CloseConnection();
            return services;
        }

        public List<Service> GetServices()
        {
            String SQL = "SELECT * FROM Services";
            return GetServices(SQL);
        }

        public List<Service> GetServicesByCategory(int idCategory)
        {
            _dbMaster.OpenConnection();
            List<Service> services = new List<Service>();
            try
            {
                MySqlCommand command = _dbMaster.GetConnection().CreateCommand();
                command.CommandText = "SELECT * FROM Services WHERE id_category = @idCategory";
                command.Parameters.AddWithValue("@idCategory", idCategory);
                command.ExecuteNonQuery();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        services.Add(new Service(Convert.ToInt32(reader["id"]),
                            reader["name"].ToString(),
                            reader["price"].ToString(),
                            reader["notation"].ToString(),
                            reader["duration"].ToString(),
                            reader["path_to_image"].ToString(),
                            Convert.ToInt32(reader["id_category"]),
                            Convert.ToInt32(reader["id_subcategory"])));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _dbMaster.CloseConnection();
            return services;
        }





    }
}