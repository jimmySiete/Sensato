using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate
{
    public class TranslateException : Exception
    {
        public string code { get; set; }
        public string messageCode { get; set; }

        public TranslateException()
        {

        }

        public TranslateException(string code, string messageCode)
        {
            this.code = code;
            this.messageCode = messageCode;
        }
    }
}
