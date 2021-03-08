
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
