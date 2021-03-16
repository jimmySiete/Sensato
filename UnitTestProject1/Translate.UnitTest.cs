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
        public void TestMethod01() // Modelo que no es de tipo XML
        {
            // var
            csSelect selectModel = new csSelect();
            //act
            //arrange
            //string csObject = XMLToCSharp.TranslateToCSharp();
        }

        [TestMethod]
        public void TestMethod02() // modelo de valor nulo
        {
            //var
            csXML model = new csXML();
            String Code = ErrorAndExceptionsCatalog._702_Code;
            String Message = ErrorAndExceptionsCatalog._702_ModelNotFound;
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
        public void TestMethod03() // version incorrecta
        {
            csXML model = new csXML();
            model.version = "2.0";
            String Code = ErrorAndExceptionsCatalog._703_Code;
            String Message = ErrorAndExceptionsCatalog._703_InvalidVersionModel;
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
        public void TestMethod04() // tipo de codificación invalido
        {
            csXML model = new csXML();
            model.version = "1.0";
            model.encoding = "ATF-42";
            String Code = ErrorAndExceptionsCatalog._704_Code;
            String Message = ErrorAndExceptionsCatalog._704_InvalidEncoding;
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
        public void TestMethod05() // elemento no encontrado
        {
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = null;
            String Code = ErrorAndExceptionsCatalog._705_Code;
            String Message = ErrorAndExceptionsCatalog._705_ElementNotFound;
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
        public void TestMethod06() // no se creó el nodo
        {
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            model.document.references.Add(new csReferences() { name = "System.Text;" });
            model.document.csNamespace.Classes = null;
            String Code = ErrorAndExceptionsCatalog._706_Code;
            String Message = ErrorAndExceptionsCatalog._706_NotCreatedNode;
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
        public void TestMethod07()
        {
            
        }

        [TestMethod]
        public void TestMethod08()
        {
            
        }

        [TestMethod]
        public void TestMethod09()
        {
            
        }

        [TestMethod]
        public void TestMethod010() // nodo sin nodos hijos
        {
            csXML model = new csXML("1.0", "UTF-8");
            model.document.references = new List<csReferences>();
            model.document.references.Add(new csReferences { name = "System;" });
            model.document.references.Add(new csReferences { name = "System.Collections.Generic;" });
            model.document.references.Add(new csReferences() { name = "System.Text;" });
            model.document.csNamespace.Classes = null;
            model.document.csNamespace = new csNamespace() { name = "Sensato.Translate.Entities" };
            model.document.csNamespace.Classes = new List<csClass>();
            model.document.csNamespace.Classes.Add(new csClass() { inheritance = "BaseSample", name = "Sample0", modifiers = "public", partial = "true", lines = null, constructors = null });
            String Code = ErrorAndExceptionsCatalog._710_Code;
            String Message = ErrorAndExceptionsCatalog._710_ChildNodesNotFound;
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
        public void TestMethod011()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod012()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod013()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod014()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod015()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod016()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod017()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod018()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod019()
        {
            //var
            //act
            //arrange
        }

        [TestMethod]
        public void TestMethod020()
        {
            //var
            //act
            //arrange
        }
    }
}
