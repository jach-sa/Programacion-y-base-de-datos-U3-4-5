using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace ChefRecetasCS.DAL
{
    public class Conexion
    {
        private const string CS =
            "server=localhost;" +
    "port=3306;" +
    "database=chef_recetas;" +
    "uid=chefuser;" +
    "pwd=Chef1234!;" +
    "charset=utf8mb4;";
        public static MySqlConnection GetConexion()
            => new MySqlConnection(CS);

    }
}
