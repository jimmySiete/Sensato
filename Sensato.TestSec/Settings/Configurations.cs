using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.TestSec
{
    public class Configurations
    {
        public static string ConnectionString
        {
            get {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
    }
}
