using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csVar : csLine
    {
        public string name { get; set; }
        public string modifier { get; set; } // si es piblica  privada, el valor puede ir o no
        public bool isStatic { get; set; } // 
        public object value { get; set; } // puede llevar una instancia o llamada a un metodo
        public string type { get; set; } // tipo de dato, depende de la linea se agrega o no
        //public csClass classes { get; set; } 
        public csGetter getter { get; set; } // crear un inicializador 
        public csSetter setter { get; set; } // 
        public List<csExecuteMethods> methods { get; set; }

        public csVar()
        {
            this.getter = null;
            this.setter = null;
            //this.classes = new csClass();
            this.methods = new List<csExecuteMethods>();
        }
    }
}
