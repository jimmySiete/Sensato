using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate.Entities
{
    public class csWhere : csLinq
    {
        public string condition { get; set; }
        public csLine line { get; set; }

        public csWhere()
        {
            this.line = new csLine(); 
        }
    }
}
