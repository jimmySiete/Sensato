using System.Collections.Generic;
using System.Linq;
using Sensato.Translate;
using Sensato.Translate.Entities;

namespace SENSATO.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<csVar> variables = new List<csVar>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "{ get; set; }", type= "string", lineCode = ""});

            csXML xmlModel = new csXML("1.0", "UTF-8");
  
            xmlModel.document.references = new List<csReferences>();
            xmlModel.document.references.Add(new csReferences { name = "System;" });
            xmlModel.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            xmlModel.document.references.Add(new csReferences() { name = "System.Text;" });
            xmlModel.document.csNamespace = new csNamespace() { name = "Sensato.Translate.Entities" };
            xmlModel.document.csNamespace.Classes = new List<csClass>();
            xmlModel.document.csNamespace.Classes.Add(new csClass { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true" });
            xmlModel.document.csNamespace.lines = new List<csLine>();
            xmlModel.document.csNamespace.lines.AddRange(variables);
            string csObject = XMLToCSharp.TranslateToCSharp(xmlModel);
        }

        //static void Main(string[] args)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    XmlElement root = doc.DocumentElement;
        //    doc.InsertBefore(xmlDeclaration, root);

        //    XmlElement element1 = doc.CreateElement(string.Empty, "cuerpo", string.Empty);
        //    doc.AppendChild(element1);
        //    XmlElement element2 = doc.CreateElement(string.Empty, "nivel1", string.Empty);
        //    element1.AppendChild(element2);
        //    XmlElement element3 = doc.CreateElement(string.Empty, "nivel2", string.Empty);
        //    element2.AppendChild(element3);

        //    XmlText text1 = doc.CreateTextNode("texto");
        //    element3.AppendChild(text1);
        //    element2.AppendChild(element3);

        //    XmlElement element4 = doc.CreateElement(string.Empty, "nivel3", string.Empty);
        //    XmlText text2 = doc.CreateTextNode("más texto");
        //    element4.AppendChild(text2);
        //    element2.AppendChild(element4);

        //    doc.Save("C:/Users/Carolina Martinez/Desktop/XMLSample.xml");
        //}



        //static void Main(string[] args)
        //{
        //    string query = @"select count(*) 
        //    from Albums";
        //    var ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
        //    DataTable dt = DataAccessADO.GetDataTable(query,CommandType.Text,null, ConnectionStr);
        //}
    }
}
