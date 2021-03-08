using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate.Entities
{
    public class csSelect: csLinq
    {
        public csClass instance { get; set; }

        public csSelect()
        {
            this.instance = new csClass();
        }
    }
}
