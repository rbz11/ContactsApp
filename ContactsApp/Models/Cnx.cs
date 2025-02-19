using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsApp.Models
{
    // Class responsible for managing the MySQL database connection
    public class Cnx
    {
        // Connection string containing the details required to connect to the MySQL database
        private static string cnx = "Server=localhost;Database=project;UserId=root;Password=1234";

        // Method to get a new MySqlConnection instance
        public static MySqlConnection getCnx()
        {
            return new MySqlConnection(cnx); // Returns a new connection object using the connection string
        }
    }
}
