using Sensato.DataAccess;
using Sensato.GenerateCSharp.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Sensato.GenerateCSharp.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        public DB_GeneratorEntities db = new DB_GeneratorEntities();

        public void GetResultSet(string SP_name, string ConnStr, int idObject)
        {
            // obtener mediante idObj la lista de parametros
            //convertir a sql parameters: valores dummies (decimal p/e: 0.0)

            List<Tb_Parameters> parameters;
            List<SqlParameter> queryParams = new List<SqlParameter>();
            parameters = db.Tb_Parameters.Where(x => x.ID_Object == idObject).ToList();
            foreach (var item in parameters)
            {
                queryParams.Add(new SqlParameter(item.ParameterName, item.DataType));
            }

            int consecutiveNumbr = 1;
            DataSet DS = DataAccessADO.GetDataSet(SP_name, CommandType.StoredProcedure, queryParams, ConnStr, null); // tercer paso
            //List<Tb_ResultSets> RS = new List<Tb_ResultSets>();
            List<Tb_ResultSetColumns> RSC = new List<Tb_ResultSetColumns>();


            foreach (DataTable resultSet in DS.Tables)  // itero de DS casa DT
            {
                Tb_ResultSets tbResult = new Tb_ResultSets();
                tbResult.ID_Object = idObject;
                tbResult.ResultSetName = "SET " + consecutiveNumbr;
                //RS.Add(tbResult);
                db.Tb_ResultSets.Add(tbResult);
                db.SaveChanges();

                foreach (DataColumn column in resultSet.Columns) // decada DT, itero cada DC
                {
                    Tb_ResultSetColumns tbColumn = new Tb_ResultSetColumns();
                    tbColumn.ID_ResultSet = tbResult.ID_ResultSet;
                    tbColumn.ParameterName = column.ColumnName;
                    tbColumn.DataType = column.DataType.Name;
                    tbColumn.Length = column.MaxLength;
                    tbColumn.Nullable = column.AllowDBNull;
                    tbColumn.IsOut = false;
                    tbColumn.Presition = 0;

                    RSC.Add(tbColumn);
                }
                consecutiveNumbr++;
            }
            //db.Tb_ResultSets.AddRange(RS); // almaceno los Result Sets
            //db.SaveChanges();

            db.Tb_ResultSetColumns.AddRange(RSC); // almaceno los Result Set Columns
            db.SaveChanges();
        } 
    }
}