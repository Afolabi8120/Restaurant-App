using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Restaurant_Software
{
    class ClassDB
    {
        public string getConnection()
        {
            string cn = "server = localhost; username = root; password = (Afolabi8120); database = restaurant;";
            return cn;
        }
    }
}
