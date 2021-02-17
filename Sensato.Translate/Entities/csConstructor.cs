using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensato.Translate.Entities
{
    public class csConstructor: csLine
    {
        public csClass classConstructor { get; set; }
        public List<csArgument> csArguments { get; set; }
        public List<csLine> csLines { get; set; }

    }
}
