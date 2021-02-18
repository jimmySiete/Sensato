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
            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "{ get; set; }", type= "string", lineCode = "", methods = null, getter = null, setter = null});
            variables.Add(new csVar() { name = "modifier", modifier = "public", isStatic = false, line = 1, value = "{ get; set; }", type = "string", lineCode = "" });
            variables.Add(new csVar() { name = "isStatic", modifier = "public", isStatic = false, line = 2, value = "{ get; set; }", type = "bool", lineCode = ""});
            variables.Add(new csVar() { name = "value", modifier = "public", isStatic = false, line = 3, value = "{ get; set; }", type = "object", lineCode = "" });
            variables.Add(new csVar() { name = "type", modifier = "public", isStatic = false, line = 4, value = "{ get; set; }", type = "string", lineCode = ""});
            variables.Add(new csVar() { name = "getter", modifier = "public", isStatic = false, line = 5, value = "{ get; set; }", type = "csGetter", lineCode = "" });
            variables.Add(new csVar() { name = "setter", modifier = "public", isStatic = false, line = 6, value = "{ get; set; }", type = "csSetter", lineCode = ""});
            variables.Add(new csVar() { name = "methods", modifier = "public", isStatic = false, line = 7, value = "{ get; set; }", type = "List<csExecuteMethods>", lineCode = "" });

            csXML xmlModel = new csXML("1.0", "UTF-8");
  
            xmlModel.document.references = new List<csReferences>();
            xmlModel.document.references.Add(new csReferences { name = "System;" });
            xmlModel.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            xmlModel.document.references.Add(new csReferences() { name = "System.Text;" });
            xmlModel.document.csNamespace = new csNamespace() { name = "Sensato.Translate.Entities" };
            xmlModel.document.csNamespace.Classes = new List<csClass>();
            xmlModel.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables });
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
