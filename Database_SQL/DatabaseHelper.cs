using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SQLite;
using Dapper;

namespace TutorBookings.Database_SQL
{
    public class DatabaseHelper
    {
        private static string connString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        
        public static SQLiteConnection Connect()
        {
            var connection = new SQLiteConnection(connString);
            connection.Open();
            return connection;
        }
    }
}