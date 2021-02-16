using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate.Entities
{
    public class csMethods
    { 
        public List<csArgument> arguments { get; set; }
        public string modifier { get; set; }
        public string datatypeReturn { get; set; }
        public List<csLine> line { get; set; }
        public bool isStatic { get; set; }
        public csLine returnedType { get; set; }
        public bool isReturned { get; set; }

        public csMethods()
        {
            this.line = new List<csLine>();
            this.arguments = new List<csArgument>();
            this.returnedType = new csLine();
        }
    }
}
