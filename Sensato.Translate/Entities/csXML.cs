using System;
using System.Collections.Generic;
using System.Text;

namespace Sensato.Translate.Entities
{
     public class csXML
    {
        public string version { get; set; }
        public string encoding { get; set; }
        public csDocument document { get; set; }

        public csXML()
        {
            this.document = new csDocument();
        }

        public csXML(string version, string encoding)
        {
            this.version = version;
            this.encoding = encoding;
            this.document = new csDocument();
        }
    }

}
