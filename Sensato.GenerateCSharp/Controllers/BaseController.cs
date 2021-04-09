using Sensato.DataAccess;
using Sensato.GenerateCSharp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sensato.GenerateCSharp.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base

        public DB_GeneratorEntities db = new DB_GeneratorEntities();

        //recibir el nombre del objeto
        //con dataAccess ejecuto el SP que seleccioné en la vista
        // se almacena en un DS, se itera cada DT, y dentro de este se itera con data column
        // resultName usar un consecutivo p/e: SET1, SET2, etc. PUEDE SER MODIFICABLE EL NOMBRE (COLOCAR LA FUNCIONALIDAD) edit, details (parametros result set cols)
        // alamaceno el RS, dentro de data column usar parameter name y con ese lleno en la BD

        public static void GetResultSet(string SP_name, string ConnStr)
        {
            int consecutiveNumbr = 1;
            Tb_ResultSets TbResults = new Tb_ResultSets();
            TbResults.ResultSetName = "SET " + consecutiveNumbr;
            DataTable dt = DataAccessADO.GetDataTable(SP_name,CommandType.StoredProcedure, null,ConnStr, null);
            List<Tb_ResultSets> RS = new List<Tb_ResultSets>();
            foreach ()
            {

            }
        }
    }
}