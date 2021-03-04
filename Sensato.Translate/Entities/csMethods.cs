using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate.Entities
{
    public class csMethods : csLine
    { 
        public string name { get; set; }
        public List<csArgument> arguments { get; set; }
        public string modifier { get; set; }
        public string dataTypeReturn { get; set; }
        public List<csLine> lines { get; set; } 
        public bool isStatic { get; set; }
        public csLine returnedType { get; set; }
        public bool isReturned { get; set; }

        public csMethods()
        {
            this.lines = new List<csLine>();
            this.arguments = new List<csArgument>();
            this.returnedType = new csLine();
        }
    }
}
