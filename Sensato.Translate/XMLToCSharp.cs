using Sensato.Translate.Entities;
using System;
using System.Xml;


namespace Sensato.Translate
{
    public class XMLToCSharp
    {
        public static string TranslateToCSharp(csXML model)
        {
            XmlDocument xmlDocument = SerializeToXML(model);
            string csSharpModel = SerializeToCSharp(xmlDocument);
            return string.Empty;
        }

        private static XmlDocument SerializeToXML(csXML model)
        {
            XmlDocument newDocument = new XmlDocument();
            XmlDeclaration declaration = newDocument.CreateXmlDeclaration(model.version, model.encoding, null);
            XmlElement heading = newDocument.DocumentElement;
            newDocument.InsertBefore(declaration, heading);

            XmlElement references = newDocument.CreateElement(null,null,null);

            if (model.document.references.Count>0)
            {
                for (var i=0; i < model.document.references.Count; i++)
                {
                    
                }
            }
            return new XmlDocument();
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            return string.Empty;
        }
    }
}
