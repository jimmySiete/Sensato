
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sensato.DataAccess
{
    public class DataAccessADO
    {
        /// <summary>
        /// Call to Stored Procedure or a Query
        /// </summary>
        /// <param name="storedProcedureOrQuery"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="connStr"></param>
        /// <param name="transaction"></param>
        /// <returns> We obtain a unique result from the SELECT statement contained in a DataTable. </returns>
        public static DataTable GetDataTable(string storedProcedureOrQuery, CommandType type, List<SqlParameter> parameters = null, string connStr = null, SqlTransaction transaction = null)
        {
            try
            {
                //DataAccessADO.ParamsAreValid(storedProcedureOrQuery,type,parameters,connStr,transaction);
                
                SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
                if (parameters != null)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.GetDataTable(cmd);
            }
            catch(Exception ex)
            {
             DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                throw new DataAccessException(ErrorsAndExceptionsCatalog._617_Code, ErrorsAndExceptionsCatalog._617_ErrorNotHandled, ex);
            }
        }

        /// <summary>
        /// Call to Stored Procedures or Queries.
        /// </summary>
        /// <param name="storedProcedureOrQuery"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="connStr"></param>
        /// <param name="transaction"></param>
        /// <returns> We obtain multiple results from the SELECT statement contained in a DataSet. </returns>
        public static DataSet GetDataSet(string storedProcedureOrQuery, CommandType type, List<SqlParameter> parameters = null, string connStr = null, SqlTransaction transaction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.GetType().Name == "StoredProcedure")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._601_Code, ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure);
                else if (!storedProcedureOrQuery.StartsWith("B") && type.GetType().Name == "StoredProcedure")// que no contenga una sentencia
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._602_Code, ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound);
                if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.GetType().Name == "Query")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._603_Code, ErrorsAndExceptionsCatalog._603_InvalidQuery);
                else if (!storedProcedureOrQuery.StartsWith("S") && type.GetType().Name == "Query")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._604_Code, ErrorsAndExceptionsCatalog._604_QueryNotFound);
                if (string.IsNullOrEmpty(type.ToString()))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidCommandType);
                else if (type.GetType().Name != "Text" || type.GetType().Name != "StoredProcedure")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._614_Code, ErrorsAndExceptionsCatalog._614_CommandTypeNotFound);
                if (parameters.All(null))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._615_Code, ErrorsAndExceptionsCatalog._615_ParametersNotFound);
                if (string.IsNullOrEmpty(connStr))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound);
                else if (!connStr.StartsWith("d")) // deberia ser que empiece con otra que no sea 'd'
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606__InvalidConnectionString);
                if (string.IsNullOrEmpty(transaction.ToString()))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._608_Code, ErrorsAndExceptionsCatalog._608_SQLTransactionNotFound);

                SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
                if (parameters != null)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.GetDataSet(cmd);
            }
            catch
            {
                throw new DataAccessException(ErrorsAndExceptionsCatalog._610_Code, ErrorsAndExceptionsCatalog._610_InvalidDataSet);
            }
        }

        /// <summary>
        /// Call to Stored Procedures or Queries used to INSERT/UPDATE rows.
        /// </summary>
        /// <param name="storedProcedureOrQuery"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="connStr"></param>
        /// <param name="transaction"></param>
        /// <returns> We can obtain results like: True if the statement was executed or False if it failed. </returns>
        public static bool ExecuteNonQuery(string storedProcedureOrQuery, CommandType type, List<SqlParameter> parameters = null, string connStr = null, SqlTransaction transaction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.GetType().Name == "StoredProcedure")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._601_Code, ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure);
                else if (!storedProcedureOrQuery.StartsWith("B") && type.GetType().Name == "StoredProcedure")// que no contenga una sentencia
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._602_Code, ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound);
                if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.GetType().Name == "Query")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._603_Code, ErrorsAndExceptionsCatalog._603_InvalidQuery);
                else if (!storedProcedureOrQuery.StartsWith("S") && type.GetType().Name == "Query")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._604_Code, ErrorsAndExceptionsCatalog._604_QueryNotFound);
                if (string.IsNullOrEmpty(type.ToString()))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidCommandType);
                else if (type.GetType().Name != "Text" || type.GetType().Name != "StoredProcedure")
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._614_Code, ErrorsAndExceptionsCatalog._614_CommandTypeNotFound);
                if (parameters.All(null))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._615_Code, ErrorsAndExceptionsCatalog._615_ParametersNotFound);
                if (string.IsNullOrEmpty(connStr))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound);
                else if (!connStr.StartsWith("d")) // deberia ser que empiece con otra que no sea 'd'
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606__InvalidConnectionString);
                if (string.IsNullOrEmpty(transaction.ToString()))
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._608_Code, ErrorsAndExceptionsCatalog._608_SQLTransactionNotFound);

                SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
                if (parameters != null)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.ExecuteNonQuery(cmd); 
            }
            catch 
            {
                throw new DataAccessException(ErrorsAndExceptionsCatalog._611_Code, ErrorsAndExceptionsCatalog._611_InvalidSentenceExecution);
            }
        }

        
        /// <summary>
        /// Method used by validation of params
        /// </summary>
        /// <param name="storedProcedureOrQuery"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="connStr"></param>
        /// <param name="transaction"></param>
        private static void ParamsAreValid(string storedProcedureOrQuery, CommandType type, List<SqlParameter> parameters, string connStr, SqlTransaction transaction)
        {
            if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.ToString() == "StoredProcedure")
               throw new DataAccessException(ErrorsAndExceptionsCatalog._602_Code, ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound);
            else if (!storedProcedureOrQuery.StartsWith("U") && type.ToString() == "StoredProcedure")// que no contenga una sentencia
                throw new DataAccessException(ErrorsAndExceptionsCatalog._601_Code, ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure);
            if (string.IsNullOrEmpty(storedProcedureOrQuery) && type.ToString() == "Text")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._604_Code, ErrorsAndExceptionsCatalog._604_QueryNotFound);
            else if (!storedProcedureOrQuery.StartsWith("S") && type.ToString() == "Text")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._603_Code, ErrorsAndExceptionsCatalog._603_InvalidQuery);
            if (string.IsNullOrEmpty(type.ToString()))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidCommandType);
            else if (type.ToString() != "Text" || type.ToString() != "StoredProcedure")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._614_Code, ErrorsAndExceptionsCatalog._614_CommandTypeNotFound);
            if (parameters.All(null))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._615_Code, ErrorsAndExceptionsCatalog._615_ParametersNotFound);
            if (string.IsNullOrEmpty(connStr))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound);
            else if (!connStr.StartsWith("d")) // deberia ser que empiece con otra que no sea 'd'
                throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606__InvalidConnectionString);
        }
    }
}
