using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Event_Management_Application
{
    internal class Database
    {


        private static Database instance;
        public string path = @"Data Source=LAPTOP-S59FH6QM\SQLEXPRESS;Initial Catalog=EventManagement;Integrated Security=True";
        public SqlConnection conn;
        public SqlCommand cmd;
        private Database() { }

        public static Database getInstance()
        {
            if (instance == null)
            {
                instance = new Database();
            }
            return instance;
        }

    }
}
