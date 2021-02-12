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
            XmlDocument newFile = new XmlDocument();  // doc creation
            XmlDeclaration declaration = newFile.CreateXmlDeclaration(model.version, model.encoding, null);
            XmlElement heading = newFile.DocumentElement; //xml declaration
            newFile.InsertBefore(declaration, heading);

            XmlElement document = newFile.CreateElement("document",string.Empty); // doc structure
            newFile.AppendChild(document);

            XmlElement references = newFile.CreateElement("references", string.Empty); // level 1 first child
            document.AppendChild(references);

            XmlElement namespaces = newFile.CreateElement("namespace", string.Empty);  // level 1 second child
            document.InsertAfter(namespaces,references);

            XmlElement usings = newFile.CreateElement("using", string.Empty); // level 2 first child
            references.AppendChild(usings);

            XmlElement classes = newFile.CreateElement("class", string.Empty); // level 2 second child
            namespaces.AppendChild(classes);

            if (model.document.references.Count>0)
            {
                for (var i=0; i < model.document.references.Count; i++)
                {
                    
                }
            }

            if ()
            {

            }
            return newFile;
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            return string.Empty;
        }
    }
}
