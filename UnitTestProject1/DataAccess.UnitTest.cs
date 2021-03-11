using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sensato.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{ 
    [TestClass]
    public class DataAccess
    {
        [TestMethod]
        public void TestMethod01() // para comprobar que un procedimiento almacenado funciona
        {
            // var
            string query = "";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._601_Code;
            String Message = ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure;
            DataAccessException resultEX = new DataAccessException();
            //act
            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr);
            }
            catch(DataAccessException ex)
            {
                resultEX = ex;
            }
            // arrange
            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod02()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod03()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod04()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod05()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod06()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod07()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod08()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod09()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod010()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod011()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod012()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod013()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod014()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod015()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod016()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod017()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod018()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod019()
        {
            // var 
            // act
            // arrange
        }

        [TestMethod]
        public void TestMethod020()
        {
            // var 
            // act
            // arrange
        }
    }
}
