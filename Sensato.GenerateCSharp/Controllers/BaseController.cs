using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sensato.GenerateCSharp.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }

        //recibir el nombre del objeto
        //con dataAccess ejecuto el SP que seleccioné en la vista
        // se almacena en un DS, se itera cada DT, y dentro de este se itera con data column
        // resultName usar un consecutivo p/e: SET1, SET2, etc.
        // alamaceno el RS, dentro de data column usar parameter name y con ese lleno en la BD
    }
}