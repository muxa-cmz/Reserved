using System;
using MySql.Data.MySqlClient;

namespace Reserved.Models.Masters
{
    public class DBMaster
    {
        private MySqlConnection _myConnection = null;
        public void OpenConnection()
        {
            String Connect = "Database=Reserved;" +
                             "Data Source=localhost;" +
                             "User Id=root;" +
                             "Password=; " +
                             "charset=utf8";
            _myConnection = new MySqlConnection(Connect);
            _myConnection.Open();
        }

        public MySqlConnection GetConnection()
        {
            return _myConnection;
        }

        public void CloseConnection()
        {
            _myConnection.Close();
        }
    }
}