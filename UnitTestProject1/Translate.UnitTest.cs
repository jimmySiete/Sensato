using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sensato.Translate;
using Sensato.Translate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    [TestClass]
    public class Translate
    {
        [TestMethod]
        public void TestMethod01() // modelo de valor nulo
        {
            //var
            csXML model = null;
            String Code = ErrorAndExceptionsCatalog._701_Code;
            String Message = ErrorAndExceptionsCatalog._701_ModelNotFound;
            TranslateException resultEX = new TranslateException();
            //act
            try {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }
            //arrange
            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod02() // version incorrecta
        {
            csXML model = new csXML();
            model.version = "2.0";
            model.encoding = "UTF-8";
            String Code = ErrorAndExceptionsCatalog._702_Code;
            String Message = ErrorAndExceptionsCatalog._702_InvalidVersionModel;
            TranslateException resultEX = new TranslateException();
            
            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }
            
            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod03() // tipo de codificación invalido
        {
            csXML model = new csXML();
            model.version = "1.0";
            model.encoding = "ATF-42";
            String Code = ErrorAndExceptionsCatalog._703_Code;
            String Message = ErrorAndExceptionsCatalog._703_InvalidEncoding;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod04() // informacion incompleta
        {
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.csNamespace.name = "";
            String Code = ErrorAndExceptionsCatalog._704_Code;
            String Message = ErrorAndExceptionsCatalog._704_NotEnoughInformation;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod05() // no se creó el nodo
        {   
            List<csLine> variables = new List<csLine>();
            variables.Add(null);

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(null);

            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(null);
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { name = "clase1", partial = "false", inheritance = "false", lines = variables, constructors = constructorList, modifiers = null });
            String Code = ErrorAndExceptionsCatalog._705_Code;
            String Message = ErrorAndExceptionsCatalog._705_NotCreatedNode;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06() // no se encontraron nodos en referencias
        {
            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() {  name = "clase1", partial = "false", inheritance = "false", lines = variables, constructors = constructorList, modifiers = null });
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06_1() // no se encontraron nodos en clases
        {
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });

            model.document.csNamespace.Classes = new List<csClass>();
            
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06_2() // no se encontraron nodos en constructores
        {
            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "modifier", modifier = "public", isStatic = true, line = 1, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csVar() { name = "isStatic", modifier = "public", isStatic = true, line = 2, value = "", type = "bool", lineCode = "", getterOrSetter = true });

            List<csConstructor> constructorList = new List<csConstructor>();

            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });

            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06_3() // no se encontraron nodos en lineas
        {
            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod07() // valor del atributo nulo
        {
            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod08() // un solo nodo, mal seleccionado
        {

        }

        [TestMethod]
        public void TestMethod09() // nombre de la clase principal nulo
        {
      
        }

        [TestMethod]
        public void TestMethod010() // nombre de la clase principal con raracteres especiales
        {
            List<csLine> methodLines = new List<csLine>();
            methodLines.Add(new csLine() { line = 0, lineCode = "var x = 5;", executeMethods = null });

            List<csArgument> methodsArgs = new List<csArgument>();
            methodsArgs.Add(new csArgument() { type = "string", value = "Saludo" });

            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csMethods() { name = "metodo", arguments = methodsArgs, isStatic = true, isReturned = true, dataTypeReturn = "string", lines = methodLines, line = 9 });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.csNamespace.name = "UltimateTests";
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "!Carly", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });

            String Code = ErrorAndExceptionsCatalog._710_Code;
            String Message = ErrorAndExceptionsCatalog._710_MainClassInvalidName;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod011() // metodo sin nombre
        {
            List<csLine> methodLines = new List<csLine>();
            methodLines.Add(new csLine() { line = 0, lineCode = "var x = 5;", executeMethods = null });

            List<csArgument> methodsArgs = new List<csArgument>();
            methodsArgs.Add(new csArgument() { type = "string", value = "Saludo" });

            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csMethods() { arguments = methodsArgs, isStatic = true, isReturned = true, dataTypeReturn = "string", lines = methodLines, line = 9 });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.csNamespace.name = "UltimateTests";
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });

            String Code = ErrorAndExceptionsCatalog._711_Code;
            String Message = ErrorAndExceptionsCatalog._711_NameMethodNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod012() // traducción fallida
        {
            List<csLine> methodLines = new List<csLine>();
            methodLines.Add(new csLine() { line = 0, lineCode = "var x = 5;", executeMethods = null });

            List<csArgument> methodsArgs = new List<csArgument>();
            methodsArgs.Add(new csArgument() { type = "string", value = "Saludo" });

            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csMethods() {name = "method", arguments = null, isStatic = true, isReturned = true, dataTypeReturn = "string", lines = methodLines, line = 9 });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "Sample0" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.csNamespace.name = "UltimateTests";
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });

            String Code = ErrorAndExceptionsCatalog._712_Code;
            String Message = ErrorAndExceptionsCatalog._712_DocumentFormatNotCreated;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod013() // constructor sin nombre de clase
        {
            List<csLine> methodLines = new List<csLine>();
            methodLines.Add(new csLine() { line = 0, lineCode = "var x = 5;", executeMethods = null });

            List<csArgument> methodsArgs = new List<csArgument>();
            methodsArgs.Add(new csArgument() { type = "string", value = "Saludo" });

            List<csArgument> argList = new List<csArgument>();
            argList.Add(new csArgument() { type = "string", value = "Apellido" });

            List<csLine> variables = new List<csLine>();
            variables.Add(new csVar() { name = "name", modifier = "public", isStatic = false, line = 0, value = "", type = "string", lineCode = "", getterOrSetter = true });
            variables.Add(new csMethods() { name = "method", arguments = null, isStatic = true, isReturned = true, dataTypeReturn = "string", lines = methodLines, line = 9 });

            List<csConstructor> constructorList = new List<csConstructor>();
            constructorList.Add(new csConstructor() { classConstructor = new csClass() { name = "" }, csArguments = argList, csLines = null });

            csXML model = new csXML("1.0", "UTF-8");
            model.document.csNamespace.name = "UltimateTests";
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = variables, constructors = constructorList });

            String Code = ErrorAndExceptionsCatalog._713_Code;
            String Message = ErrorAndExceptionsCatalog._713_NameClassNotFound;
            TranslateException resultEX = new TranslateException();

            try
            {
                string csObject = XMLToCSharp.TranslateToCSharp(model);
            }
            catch (TranslateException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod014() // caracteres especiales dentro del nombre de la clase
        {
          
        }

        [TestMethod]
        public void TestMethod015() // el metodo no retorna un valor
        {
        
        }
    }
}
