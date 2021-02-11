using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csGetter
    {
        public List<csLine> csLine { get; set; }

        public csGetter()
        {
            this.csLine = new List<csLine>();
        }
    }
}
