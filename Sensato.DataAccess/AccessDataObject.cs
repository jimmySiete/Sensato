
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
            if (string.IsNullOrEmpty(storedProcedureOrQuery))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._601_Code, ErrorsAndExceptionsCatalog._601_InvalidStoredProcedure);
            else if (storedProcedureOrQuery.StartsWith("s"))// que no contenga una sentencia
                throw new DataAccessException(ErrorsAndExceptionsCatalog._602_Code, ErrorsAndExceptionsCatalog._602_StoredProdecureNotFound);

            if (string.IsNullOrEmpty(type.ToString()))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._604_Code, ErrorsAndExceptionsCatalog._604_InvalidCommandType);
            else if (type.GetType().Name != "Text"|| type.GetType().Name != "StoredProcedure")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._603_Code, ErrorsAndExceptionsCatalog._603_CommandTypeNotFound);

            if (parameters.All(null))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._605_Code, ErrorsAndExceptionsCatalog._605_InvalidParameters);
            else if (parameters.GetType().Name != "SqlParameter")
                throw new DataAccessException(ErrorsAndExceptionsCatalog._606_Code, ErrorsAndExceptionsCatalog._606_ParametersNotFound);

            if (string.IsNullOrEmpty(connStr))
                throw new DataAccessException(ErrorsAndExceptionsCatalog._607_Code, ErrorsAndExceptionsCatalog._607__InvalidConnectionString);
            else if (connStr.("d")) // deberia ser que empiece con otra que no sea 'd'
                throw new DataAccessException(ErrorsAndExceptionsCatalog._608_Code, ErrorsAndExceptionsCatalog._608_ConnectionStringNotFound);

            SqlCommand cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            if (parameters != null)
                foreach (SqlParameter sqlParam in parameters)
                    cmd.Parameters.Add(sqlParam);
                
                return Connection.GetDataTable(cmd);
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
            var cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            if (parameters != null)
                foreach(SqlParameter sqlParam in parameters)
                    cmd.Parameters.Add(sqlParam);

                return Connection.GetDataSet(cmd);
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
            var cmd = Connection.GetConnection(storedProcedureOrQuery, type, connStr, transaction);
            if (parameters != null)
                foreach(SqlParameter sqlParam in parameters)
                    cmd.Parameters.Add(sqlParam);

                return Connection.ExecuteNonQuery(cmd);
        }
    }
}
