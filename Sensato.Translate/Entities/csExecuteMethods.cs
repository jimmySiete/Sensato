using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csExecuteMethods
    {
        public string name { get; set; }
        public bool returns { get; set; }
        public List<csArgument> csArgument { get; set; }

        public csExecuteMethods()
        {
            this.csArgument = new List<csArgument>();

            csArgument = new List<csArgument>();
        }
    }
}
