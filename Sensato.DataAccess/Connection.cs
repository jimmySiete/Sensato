using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Sensato.DataAccess
{
    internal class Connection
    {
        /// <summary>
        /// Method that stablishes connection to SQL Server.
        /// </summary>
        /// <param name="text">The chain to execute, name of the SP or the query</param>
        /// <param name="type"> The specification if we're using a SP or a text </param>
        /// <param name="connStr"></param>
        /// <param name="transaction"></param>
        /// <returns>As a result, the complete connection</returns>
        public static SqlCommand GetConnection(string text, CommandType type, string connStr, SqlTransaction transaction = null)
        {
            if (connStr == null)
                connStr = GetConnectionStringFromSettings();
            else if (!connStr.StartsWith("d"))
                connStr = GetConnectionStringFromSettings();
            if (type.ToString() != "Text" && type.ToString() != "StoredProcedure")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidCommandType);

            SqlCommand command;
            SqlConnection connection = new SqlConnection(connStr);
            command = new SqlCommand(text, connection);
            command.CommandType = type;

            if (command.Connection.ConnectionString.Length == 0)
                throw new DataAccessException(ErrorsAndExceptionsCatalog._612_Code, ErrorsAndExceptionsCatalog._612_ConnectionFailed);

            if (transaction != null)
                command.Transaction = transaction;

            return command;
        }

        /// <summary>
        /// Using the recently created connection, the statement is executed.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>In a DataTable we're getting the result from the statement</returns>
        public static DataTable GetDataTable(SqlCommand command)
        {
            DataTable dt = new DataTable();
            try
            {
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                if (dt.Columns.Count == 0)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._609_Code,ErrorsAndExceptionsCatalog._609_InvalidDataTable);
                else
                    throw ex;
            }
            finally
            {
                command.Connection.Dispose();
                command.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Using the recently created connection, the statement is executed. 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>In a DataSet we're getting the result from the statement.</returns>
        public static DataSet GetDataSet(SqlCommand command)
        {
            DataSet ds = new DataSet();
            try
            {
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                if (ds.Tables.Count == 0)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._610_Code, ErrorsAndExceptionsCatalog._610_InvalidDataSet);
                else
                    throw ex;
            }
            finally
            {
                command.Connection.Dispose();
                command.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// Once the connection is created, we use this method to execute INSERT/UPDATE operations.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>The result is a boolean response: true if the row/s were affected or false if not</returns>
        public static bool ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                command.Connection.Open();

                return (command.ExecuteNonQuery() > 0) ? true : false;
            }
            catch (Exception ex)
            {
                if (command.BeginExecuteNonQuery().IsCompleted == false)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._611_Code, ErrorsAndExceptionsCatalog._611_InvalidSentenceExecution);
                throw ex;
            }
            finally
            {
                command.Connection.Dispose();
                command.Dispose();
            }
        }

        /// <summary>
        /// This method is used to get the connection string when it isn't given.
        /// </summary>
        /// <returns>The Connection String defined from the APPConfig file. </returns>
        private static string GetConnectionStringFromSettings()
        {
            try
            {
                return ConfigurationManager.AppSettings["ConnStr"];
            }
            catch
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ConnStr"]))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound);
                else
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606__InvalidConnectionString);
            }
        }
    }
}
