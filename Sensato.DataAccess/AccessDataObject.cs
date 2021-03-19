
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
            SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            try
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if (parameters.Count > 0)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.GetDataTable(cmd);
            }
            catch(Exception ex)
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if(ex != null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
                 else if(cmd.Transaction == null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._609_Code, ErrorsAndExceptionsCatalog._609_InvalidDataTable);
                
                throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
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
            SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            try
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if (parameters != null)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.GetDataSet(cmd);
            }
            catch (Exception ex)
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if (ex != null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
                else if (cmd.Transaction == null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._609_Code, ErrorsAndExceptionsCatalog._609_InvalidDataTable);

                throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
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
            SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            try
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if (parameters != null)
                    foreach (SqlParameter sqlParam in parameters)
                        cmd.Parameters.Add(sqlParam);

                return Connection.ExecuteNonQuery(cmd); 
            }
            catch (Exception ex)
            {
                DataAccessADO.ParamsAreValid(storedProcedureOrQuery, type, parameters, connStr, transaction);
                if (ex != null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
                else if (cmd.Transaction == null)
                    throw new DataAccessException(ErrorsAndExceptionsCatalog._609_Code, ErrorsAndExceptionsCatalog._609_InvalidDataTable);

                throw new DataAccessException(ErrorsAndExceptionsCatalog._613_Code, ErrorsAndExceptionsCatalog._613_ErrorNotHandled, ex);
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
            else if (!storedProcedureOrQuery.Contains("S") && type.ToString() == "Text")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._603_Code, ErrorsAndExceptionsCatalog._603_InvalidQuery);
            if (type.ToString().ToLower() != "text" && type.ToString().ToLower() != "storedprocedure")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidCommandType);
            if (parameters.Count == 0)
                throw new DataAccessException(ErrorsAndExceptionsCatalog._614_Code, ErrorsAndExceptionsCatalog._614_ParametersNotFound);
            if (string.IsNullOrEmpty(connStr))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607_ConnectionStringNotFound);
            else if (!connStr.StartsWith("d")) // deberia ser que empiece con otra que no sea 'd'
                throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606__InvalidConnectionString);
        }
    }
}
