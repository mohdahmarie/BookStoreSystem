using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BookStoreSystem
{
    public static class DatabaseHelper
    {
       private static String ConnectionString = @"Server= localhost\SQLEXPRESS; Database= BookStoreDB; Trusted_connection=True; TrustServerCertificate=True;";

        public static SqlConnection GetConnection() {
            return new SqlConnection(ConnectionString);
        }
    }

}
