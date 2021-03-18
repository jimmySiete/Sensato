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

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = ConfigurationManager.AppSettings["ConnStr"];
            //connection.Open();
            //SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._601_Code;
            String Message = ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure;
            DataAccessException resultEX = new DataAccessException();
            //act
            try
            {
                DataTable dt = DataAccessADO.GetDataTable("CountRecordAlbums", CommandType.StoredProcedure, listparams, ConnectionStr, null);
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

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._602_Code;
            String Message = ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound;
            DataAccessException resultEX = new DataAccessException();
            
            try
            {
                DataTable dt = DataAccessADO.GetDataTable(null, CommandType.StoredProcedure, listparams, ConnectionStr,transaction);
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

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._603_Code;
            String Message = ErrorsAndExceptionsCatalog._603_InvalidQuery;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, ConnectionStr, transaction);
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
            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._604_Code;
            String Message = ErrorsAndExceptionsCatalog._604_QueryNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(null, CommandType.Text, listparams, ConnectionStr, transaction);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod05() // tipo de comando diferente
        {
            string query = "Select count(*) from Albums";
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._605_Code;
            String Message = ErrorsAndExceptionsCatalog._605_InvalidCommandType;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.TableDirect, listparams, ConnectionStr, transaction);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod06() // cadena de conexion mal estructurada
        {
            string query = "Select count(*) from Albums";
            String ConnectionStr = "user/Carolina";

            String Code = ErrorsAndExceptionsCatalog._606_Code;
            String Message = ErrorsAndExceptionsCatalog._606__InvalidConnectionString;

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, ConnectionStr, transaction);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod07() // cadena de conexion nula
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._607_Code;
            String Message = ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, null, transaction);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod08() // transaccion valor nulo
        {
            string query = "Select count(*) from Albums";
            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });
            String ConnectionStr = "data source=./;initial catalog=MvcMusicStore;integrated security=True;MultipleActiveResultSets=True;";
            String Code = ErrorsAndExceptionsCatalog._608_Code;
            String Message = ErrorsAndExceptionsCatalog._608_SQLTransactionNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams,ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod09() // no se obtuvo el dataTable
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = "data source=./;initial catalog=MvcMusicStore;integrated security=True;MultipleActiveResultSets=True;";

            String Code = ErrorsAndExceptionsCatalog._609_Code;
            String Message = ErrorsAndExceptionsCatalog._609_InvalidDataTable;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;  
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod010() // no se obtuvo del dataSet
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = "data source=./;initial catalog=MvcMusicStore;integrated security=True;MultipleActiveResultSets=True;";

            String Code = ErrorsAndExceptionsCatalog._610_Code;
            String Message = ErrorsAndExceptionsCatalog._610_InvalidDataSet;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataSet ds = DataAccessADO.GetDataSet(query, CommandType.Text, listparams, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod011() // no se pudo ejecutar la sentencia
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = "data source=./;initial catalog=MvcMusicStore;integrated security=True;MultipleActiveResultSets=True;";

            String Code = ErrorsAndExceptionsCatalog._611_Code;
            String Message = ErrorsAndExceptionsCatalog._611_InvalidSentenceExecution;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                bool result = DataAccessADO.ExecuteNonQuery(query, CommandType.Text, listparams, ConnectionStr,null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod012() // comando incompleto
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            String Code = ErrorsAndExceptionsCatalog._612_Code;
            String Message = ErrorsAndExceptionsCatalog._612_InvalidCommand;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, listparams, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod013() // comando de valor nulo
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });


            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            String Code = ErrorsAndExceptionsCatalog._613_Code;
            String Message = ErrorsAndExceptionsCatalog._613_CommandNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text,listparams, ConnectionStr, null);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod014() // tipo de comando nulo
        {
            string query = "Select count(*) from Albums";

            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter() { ParameterName = "Caro" });

            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = new CommandType();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._614_Code;
            String Message = ErrorsAndExceptionsCatalog._614_CommandTypeNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query,cmd.CommandType, listparams, ConnectionStr, transaction);
            }
            catch (DataAccessException ex)
            {
                resultEX = ex;
            }

            Assert.AreEqual(Code, resultEX.code);
            Assert.AreEqual(Message, resultEX.messageCode);
        }

        [TestMethod]
        public void TestMethod015() // parametros nulos
        {
            string query = "Select count(*) from Albums";
             
            String ConnectionStr = ConfigurationManager.AppSettings["ConnStr"];

            SqlConnection connection = new SqlConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("SampleTransaction");

            String Code = ErrorsAndExceptionsCatalog._615_Code;
            String Message = ErrorsAndExceptionsCatalog._615_ParametersNotFound;
            DataAccessException resultEX = new DataAccessException();

            try
            {
                DataTable dt = DataAccessADO.GetDataTable(query, CommandType.Text, parameters:null, ConnectionStr, transaction);
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
