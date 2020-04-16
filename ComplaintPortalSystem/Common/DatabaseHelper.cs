using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ComplaintPortalSystem.Common
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetDatabaseConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["Database.ConnectionString"];
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }
    }
}