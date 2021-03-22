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
            model.document.references = null;
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
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            model.document.references.Add(new csReferences() { name = "System.Text;" });
            model.document.csNamespace.Classes = null;
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
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() {  name = "clase1", partial = "false", inheritance = "false", lines = null, constructors = null, modifiers = null });
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
            model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            model.document.references.Add(new csReferences() { name = "System.Text;" });

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
            model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            model.document.references.Add(new csReferences() { name = "System.Text;" });

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
            argList.Add(new csArgument() { type = "string", value = "Nombre" });

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

        //[TestMethod]
        //public void TestMethod07()
        //{

        //}

        //[TestMethod]
        //public void TestMethod08()
        //{

        //}

        //[TestMethod]
        //public void TestMethod09() // nodo sin nodos hijos
        //{
        //    csXML model = new csXML("1.0", "UTF-8");
        //    model.document.references = new List<csReferences>();
        //    model.document.references.Add(new csReferences { name = "System;" });
        //    model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
        //    model.document.references.Add(new csReferences() { name = "System.Text;" });
        //    model.document.csNamespace.Classes = null;
        //    model.document.csNamespace = new csNamespace() { name = "Sensato.Translate.Entities" };
        //    model.document.csNamespace.Classes = new List<csClass>();
        //    model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = null, constructors = null });
        //    String Code = ErrorAndExceptionsCatalog._706_Code;
        //    String Message = ErrorAndExceptionsCatalog._706_ChildNodesNotFound;
        //    TranslateException resultEX = new TranslateException();

        //    try
        //    {
        //        string csObject = XMLToCSharp.TranslateToCSharp(model);
        //    }
        //    catch (TranslateException ex)
        //    {
        //        resultEX = ex;
        //    }

        //    Assert.AreEqual(Code, resultEX.code);
        //    Assert.AreEqual(Message, resultEX.messageCode);
        //}

        //[TestMethod]
        //public void TestMethod010()
        //{
        //    //var
        //    //act
        //    //arrange
        //}

        //[TestMethod]
        //public void TestMethod011()
        //{
        //    //var
        //    //act
        //    //arrange
        //}

        //[TestMethod]
        //public void TestMethod012()
        //{
        //    //var
        //    //act
        //    //arrange
        //}

        //[TestMethod]
        //public void TestMethod013()
        //{
        //    //var
        //    //act
        //    //arrange
        //}

        //[TestMethod]
        //public void TestMethod014()
        //{
        //    //var
        //    //act
        //    //arrange
        //}

        //[TestMethod]
        //public void TestMethod015()
        //{
        //    //var
        //    //act
        //    //arrange
        //}
    }
}
