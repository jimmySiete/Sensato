using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csVar
    {
        public string name { get; set; }
        public string modifier { get; set; }
        public bool statics { get; set; }
        public object value { get; set; }
        public string type { get; set; }
        public string className { get; set; }
        public csGetter getter { get; set; }
        public csSetter setter { get; set; }

        public csVar()
        {
            this.getter = null;
            this.setter = null;
        }
    }
}
