using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace CafePOS
{
    public class Database
    {
        public static string connectionString =
            "Server=localhost;Port=3306;Database=cafeposdb;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
