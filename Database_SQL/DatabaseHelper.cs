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
        public static SQLiteConnection Connect()
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath("~/App_Data/TutorDatabase");
                var connString = $"Data Source={path};Version=3;";
                var connection = new SQLiteConnection(connString);
                connection.Open();
                return connection;
            } catch (Exception ex)
            {
                throw new Exception("connection failed: " + ex.Message);
            }

        }
    }
}