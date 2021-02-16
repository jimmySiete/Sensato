using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csDocument
    {
        public List<csReferences> references { get; set; }
        public csNamespace csNamespace { get; set; }

        public csDocument()
        {
            this.references = new List<csReferences>();
            this.csNamespace = new csNamespace();
        }
    }
}
