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
        public void TestMethod01() // para comprobar que un SP o Query no sea nulo o indefinido
        {
            // var
            string query = null;
           
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._601_Code;
            String Message = ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure;
            DataAccessException resultEX = new DataAccessException();
            //act
            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text,null, ConnectionStr);
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
        public void TestMethod02() // SP o Query mal estructurado
        {
            string query = "kdfjsjdfbsdf";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._602_Code;
            String Message = ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod03() // tipo de comando incorrecto
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._604_Code;
            String Message = ErrorsAndExceptionsCatalog._604_InvalidCommandType;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.TableDirect, null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod04() // valor del tipo de comando nulo o indefinido // FALTA DE ENCONTRAR UN NULL O UNDEFINED
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._603_Code;
            String Message = ErrorsAndExceptionsCatalog._603_CommandTypeNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.TableDirect, null, ConnectionStr);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod05() // lista de parametros con valor nulo o indefinido
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._606_Code;
            String Message = ErrorsAndExceptionsCatalog._606_ParametersNotFound;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06() // parametros que no son //PENDIENTE PARAMETROS INCORRECTOS
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._605_Code;
            String Message = ErrorsAndExceptionsCatalog._605_InvalidParameters;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod07() // Cadena de conexión no valida
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = "cfgfhghghghgh";
            String Code = ErrorsAndExceptionsCatalog._607_Code;
            String Message = ErrorsAndExceptionsCatalog._607__InvalidConnectionString;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod08() // cadena de conexión con valor nulo o indefinido
        {
            string query = "select count(*) from Albums";
            String Code = ErrorsAndExceptionsCatalog._608_Code;
            String Message = ErrorsAndExceptionsCatalog._608_ConnectionStringNotFound;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod09() // valor de la transaccion nulo o indefinido
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._609_Code;
            String Message = ErrorsAndExceptionsCatalog._609_SQLTransactionNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, null, ConnectionStr ,null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod010() // valor de la transacción invalida  // PENDIENTE
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._610_Code;
            String Message = ErrorsAndExceptionsCatalog._610_InvalidSQLTransaction;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod011() // no se pudo establecer la conexion // PENDIENTE
        {
            string query = "select count(*) from Albums";
            String ConnectionStr ="";
            String Code = ErrorsAndExceptionsCatalog._611_Code;
            String Message = ErrorsAndExceptionsCatalog._611_ConnectionNotEstablished;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod012() // conexion cerrada cuando debe de estar abierta // PENDIENTE DE REVISAR
        {
            string query = "select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._612_Code;
            String Message = ErrorsAndExceptionsCatalog._612_ConnectionClosed;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod013() // no se creo la DataTable //PENDIENTE
        {
            string query = "select count(*) from books";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._613_Code;
            String Message = ErrorsAndExceptionsCatalog._613_DataTableNotCreated;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod014() // no se creo el DataSet //PENDIENTE
        {
            string query = "Select (*) from Albums";
            String Code = ErrorsAndExceptionsCatalog._614_Code;
            String Message = ErrorsAndExceptionsCatalog._614_DataSetNotCreated;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataSet ds = DataAccessADO.GetDataSet(query, CommandType.Text, null,null,null);
            }
            catch(DataAccessException ex)
            {
                resultEX = ex; 
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod015() // la sentecia no se realizó //PENDIENTE
        {
            string query = "Select 1/0";
            String Code = ErrorsAndExceptionsCatalog._615_Code;
            String Message = ErrorsAndExceptionsCatalog._615_SenteceNonExecuted;
            DataAccessException resultEX = new DataAccessException();
            bool result = false;
            try
            {
                result = DataAccessADO.ExecuteNonQuery(query, CommandType.Text,null,null,null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod016() // no se obtuvo la DataTable //PENDIENTE
        {
            string query = "select count(*) from books";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._616_Code;
            String Message = ErrorsAndExceptionsCatalog._616_InvalidDataTable;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod017() // no se obtuvo el DataSet //PENDIENTE
        {
            string query = "Select (*) from Albums";
            String Code = ErrorsAndExceptionsCatalog._617_Code;
            String Message = ErrorsAndExceptionsCatalog._617_InvalidDataSet;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataSet ds = DataAccessADO.GetDataSet(query, CommandType.Text, null, null, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod018() // no se obtuvo el resultado de la sentencia // PENDIENTE
        {
            string query = "select count(*) from books";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._618_Code;
            String Message = ErrorsAndExceptionsCatalog._618_InvalidSentenceExecution;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                bool result = DataAccessADO.ExecuteNonQuery(query, CommandType.Text, null, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod019() // comando incompleto //PENDIENTE
        {
            string query = "select count(*) from Album";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._618_Code;
            String Message = ErrorsAndExceptionsCatalog._618_InvalidSentenceExecution;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod020() // valor del comando nulo o idenfinido //PENDIENTE
        {
            string query = "select count(*) from Album";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];
            String Code = ErrorsAndExceptionsCatalog._618_Code;
            String Message = ErrorsAndExceptionsCatalog._618_InvalidSentenceExecution;
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
            Assert.AreEqual(Message, resultEX.messageCode);
        }
    }
}
