using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SpargoTech_Test.Configuration
{
    public static class ConfigWorker
    {
        public static string GetConnectionString()
        {
            try
            {              
                return ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            }
            catch
            {              
                return "";
            }
        }
    }
}


