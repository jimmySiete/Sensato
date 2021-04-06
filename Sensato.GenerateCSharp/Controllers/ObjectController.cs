using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Windows;
using Sensato.GenerateCSharp.Models;
using Sensato.DataAccess;

namespace Sensato.GenerateCSharp.Controllers
{
    public class ObjectController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: Object
        public ActionResult Index(int idContext, int idProject)
        {
            ViewBag.idContext = idContext;
            Tb_Contexts tbC = db.Tb_Contexts.Find(idContext);
            ViewBag.CxtName = tbC.ContextName;
            ViewBag.idProject = idProject;
            var tb_Objects = db.Tb_Objects.Include(t => t.Tb_Contexts);
            return View(tb_Objects.Where(x => x.ID_Context == idContext).ToList());
        }

        // GET: Object/Create
        public ActionResult Create(int idContext, int idProject)
        {
            ViewBag.idContext = idContext;
            Tb_Contexts tb = db.Tb_Contexts.Find(idContext);
            ViewBag.CxtName = tb.ContextName;
            ViewBag.idProject = idProject;
            return View();
        }

        // POST: Object/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Object,ID_Context,ObjectName")] Tb_Objects tb_Objects, int idContext, int idProject)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Tb_Objects.Add(tb_Objects);
                        //agregar fecha 
                        db.SaveChanges();

                        List<Tb_Parameters> listparams = new List<Tb_Parameters>();
                        // data accesss
                        //convierto la dt a lista
                        // a la lista le agrego el id del object
                        // agregar a la base de datos

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch(Exception ex) 
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName", tb_Objects.ID_Context);
            return View(tb_Objects);
        }

        // GET: Object/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Objects tb_Objects = db.Tb_Objects.Find(id);
            if (tb_Objects == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName", tb_Objects.ID_Context);
            ViewBag.CxtName = tb_Objects.Tb_Contexts.ContextName;
            return View(tb_Objects);
        }

        // POST: Object/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Object,ID_Context,ObjectName")] Tb_Objects tb_Objects, int idContext)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Objects).State = EntityState.Modified;
                tb_Objects.ID_Context = idContext;
                //guardar el ID DE LA CONSULTA A LA TABLA
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName", tb_Objects.ID_Context);
            return View(tb_Objects);
        }

        // GET: Object/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Objects tb_Objects = db.Tb_Objects.Find(id);
            if (tb_Objects == null)
            {
                return HttpNotFound();
            }
            Tb_Contexts tb = db.Tb_Contexts.Find(id);
            ViewBag.CxtName = tb.ContextName;
            ViewBag.idProject = tb.Tb_Projects.ID_Project;
            ViewBag.idContext = tb.ID_Context;
            return View(tb_Objects);
        }

        // POST: Object/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Objects tb_Objects = db.Tb_Objects.Find(id);
            db.Tb_Objects.Remove(tb_Objects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Metodo para obtener los ID's de la tabla sysobjects
        [HttpPost]
        public JsonResult GetStoredProceduresFromSysObjects(string txt, int idProject)
        {
            string query = "GetStoredProcedures";
            Tb_Projects tb = db.Tb_Projects.Find(idProject);
            string ConnStr;
            if (tb.LocalConnection.Value)
                ConnStr = string.Format("data source=./;initial catalog={0};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework", tb.ProjectDatabase);
            else
                ConnStr = string.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework", tb.Server, tb.ProjectDatabase, tb.ProjectUser, tb.Password); ;

            //Parameters
            List<SqlParameter> listparams = new List<SqlParameter>();
            listparams.Add(new SqlParameter("txt",txt));

            List<SelectListItem> list = new List<SelectListItem>();

            DataTable dt = DataAccessADO.GetDataTable(query,CommandType.StoredProcedure,listparams,ConnStr,null);

            list = dt.AsEnumerable().Select(x => new SelectListItem() { Text = x.Field<string>("name"), Value = x.Field<string>("id") }).ToList();

            return Json(list, JsonRequestBehavior.DenyGet);
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
