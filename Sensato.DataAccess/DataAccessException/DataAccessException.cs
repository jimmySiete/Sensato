using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.DataAccess.DataAccessException
{
    public class DataAccessException : Exception
    {
        public string code { get; set; }
        public string messageCode { get; set; }
    }
}
