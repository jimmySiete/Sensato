using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.DataAccess
{
    public class DataAccessException : Exception
    {
        public string code { get; set; }
        public string messageCode { get; set; }
        public Exception exception { get; set; }

        public DataAccessException()
        {

        }

        public DataAccessException(string code, string messageCode)
        {
            this.code = code;
            this.messageCode = messageCode;
            this.exception = new Exception();
        }

        public DataAccessException(String code, string messageCode, Exception ex)
        {
            this.code = code;
            this.messageCode = messageCode;
            this.exception = ex;
        }

    }
}
