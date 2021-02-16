using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csLine
    {
        public int line { get; set; }
        public csExecuteMethods executeMethods { get; set; }
        public csVar Variables { get; set; }
        public csMethods Methods { get; set; }

        public csLine()
        {
            this.executeMethods = new csExecuteMethods();
            this.Variables = new csVar();
            this.Methods = new csMethods();
        }
    }
}
