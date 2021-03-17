using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sensato.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace UnitTestProject1
{
    [TestClass]
    public class DataAccess
    {
        [TestMethod]
        public void TestMethod01() // SP mal estructurado
        {
            // var
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._601_Code;
            String Message = ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure;
            DataAccessException resultEX = new DataAccessException();
            //act
            try
            {
                DataTable dt = DataAccessADO.GetDataTable("CountRecordAlbums", CommandType.StoredProcedure, parameters: null, ConnectionStr);
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
        public void TestMethod02() // SP de valor nulo
        {
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._602_Code;
            String Message = ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound;
            DataAccessException resultEX = new DataAccessException();
            
            try
            {
                DataTable dt = DataAccessADO.GetDataTable(null, CommandType.StoredProcedure, parameters: null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }
            
            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod03() // query mal estructurado
        {
            string query = "delect count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._603_Code;
            String Message = ErrorsAndExceptionsCatalog._603_InvalidQuery;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, parameters:null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod04() // query de valor nulo
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._603_Code;
            //String Message = ErrorsAndExceptionsCatalog._603_CommandTypeNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(null, CommandType.Text, null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod05() // tipo de comando diferente
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._606_Code;
           // String Message = ErrorsAndExceptionsCatalog._606_ParametersNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06() // cadena de conexion mal estructurada
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._605_Code;
            //String Message = ErrorsAndExceptionsCatalog._605_InvalidParameters;
            List<SqlParameter> listparams = new List<SqlParameter>();

            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod07() // cadena de conexion nula
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = "cfgfhghghghgh";
            String Code = ErrorsAndExceptionsCatalog._607_Code;
           // String Message = ErrorsAndExceptionsCatalog._607__InvalidConnectionString;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod08() // transaccion valor nulo
        {
            string query = "select count(*) from Albums";
            String Code = ErrorsAndExceptionsCatalog._608_Code;
            //String Message = ErrorsAndExceptionsCatalog._608_ConnectionStringNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod09() // no se obtuvo el dataTable
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._610_Code;
            //String Message = ErrorsAndExceptionsCatalog._610_InvalidSQLTransaction;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;  
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod010() // no se obtuvo del dataSet
        {
            string query = "select count(*) from Albums";
            String ConnectionStr ="";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod011() // no se pudo ejecutar la sentencia
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = "";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod012() // comando incompleto
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = "";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod013() // comando de valor nulo
        {
            string query = "Select count(*) from Albums";
            String ConnectionStr = "";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod014() // tipo de comando nulo
        {
            string query = "Select count(*) from Albums";
            String ConnectionStr = "";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod015() // parametros nulos
        {
            string query = "Select count(*) from Albums";
            String ConnectionStr = "";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            //String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            //Assert.AreEqual(Message, resultEX.messageCode);
        }
    }
}
