using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sensato.GenerateCSharp.Models;

namespace Sensato.GenerateCSharp.Controllers
{
    public class ParametersController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: Parameters
        public ActionResult Index(int idObject, int idContext, int idProject)
        {
            ViewBag.idObject = idObject;
            Tb_Objects tbO = db.Tb_Objects.Find(idObject);
            ViewBag.ObjName = tbO.ObjectName;
            ViewBag.idContext = idContext;
            ViewBag.idProject = idProject;
            var tb_Parameters = db.Tb_Parameters.Include(t => t.Tb_Objects);
            return View(tb_Parameters.Where(x=>x.ID_Object == idObject).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
