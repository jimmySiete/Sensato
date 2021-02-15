using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csLine
    {
        public int line { get; set; }
        public csExecuteMethods executeMethods { get; set; }

        public csLine()
        {
            this.executeMethods = new csExecuteMethods();
        }
    }
}
