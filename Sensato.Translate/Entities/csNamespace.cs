using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csNamespace
    {
        public string name { get; set; }
        public List<csClass> Classes { get; set; }
        public List<csLine> lines { get; set; }
        public csNamespace()
        {
            this.Classes = new List<csClass>();
            this.lines = new List<csLine>();
        }
    }
}
