using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csSetter
    {
        public List<csLine> csLine { get; set; }

        public csSetter()
        {
            this.csLine = new List<csLine>();
        }
    }
}
