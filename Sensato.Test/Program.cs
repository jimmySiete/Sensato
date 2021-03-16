using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sensato.Translate;
using Sensato.Translate.Entities;

namespace SENSATO.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<csLine> methodLines = new List<csLine>();
            methodLines.Add(new csLine() { line = 0, lineCode = "var x = 5;", executeMethods = null});

            List<csArgument> methodsArgs = new List<csArgument>();
            methodsArgs.Add(new csArgument() { type = "string", value = "Saludo" });
            methodsArgs.Add(new csArgument() { type = "int", value = "Año" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "modifier", modifier = "public", isStatic = true, line = 1, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "isStatic", modifier = "public", isStatic = true, line = 2, value = "", type = "bool", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "value", modifier = "public", isStatic = true, line = 3, value = "", type = "object", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "type", modifier = "public", isStatic = true, line = 4, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "getter", modifier = "public", isStatic = true, line = 5, value = "", type = "csGetter", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "setter", modifier = "public", isStatic = true, line = 6, value = "", type = "csSetter", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "methods", modifier = "public", isStatic = true, line = 7, value = "", type = "List<csExecuteMethods>", lineCode = "", getterOrSetter = true });
            variables.Add(new csMethods() { name = "Testing", arguments = methodsArgs, isStatic = false, isReturned = false, dataTypeReturn = "void", lines = null , line = 8});
            variables.Add(new csMethods() { name = "AnotherTesting", arguments = null, isStatic = true, isReturned = true, dataTypeReturn = "string", lines = methodLines, line = 9 });

            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });
            argList.Add(new csArgument() { type = "string", value = "Nombre" });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML xmlModel = new csXML("1.0", "UTF-8");
  
            xmlModel.document.references = new List<csReferences>();
            xmlModel.document.references.Add(new csReferences { name = "System;" });
            xmlModel.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            xmlModel.document.references.Add(new csReferences() { name = "System.Text;" });
            xmlModel.document.csNamespace = new csNamespace() { name = "Sensato.Translate.Entities" };
            xmlModel.document.csNamespace.Classes = new List<csClass>();
            xmlModel.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });
            string csObject = XMLToCSharp.TranslateToCSharp(xmlModel);
        }

        //static void Main(string[] args)
        //{
        //    string query = @"select count(*) 
        //    from Albums";
        //    var ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
        //    DataTable dt = DataAccessADO.GetDataTable(query,CommandType.Text,null, ConnectionStr);
        //}
    }
}
