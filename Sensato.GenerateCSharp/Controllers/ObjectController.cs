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
    public class ObjectController : BaseController
    {
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
        public ActionResult Create([Bind(Include = "ID_Object,ID_Context,ObjectName, Entity, ObjDescription, ID_AuxObject, AuxNameObject")] Tb_Objects tb_Objects, int idContext, int idProject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tb_Objects.ID_SysObject = tb_Objects.ID_AuxObject;
                    tb_Objects.Sys_ObjectName = tb_Objects.AuxNameObject;
                    db.Tb_Objects.Add(tb_Objects);
                    db.SaveChanges();

                    //Connection String:
                    string ConnStr;
                    Tb_Projects tb = db.Tb_Projects.Find(idProject);
                    if (tb.LocalConnection.Value)
                        ConnStr = string.Format("data source=./;initial catalog={0};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework", tb.ProjectDatabase);
                    else
                        ConnStr = string.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework", tb.Server, tb.ProjectDatabase, tb.ProjectUser, tb.Password); ;

                    List<SqlParameter> queryParams = new List<SqlParameter>();
                    queryParams.Add(new SqlParameter("idObject", tb_Objects.ID_AuxObject));

                    List<Tb_Parameters> listparams = new List<Tb_Parameters>();
                    DataTable dt = DataAccessADO.GetDataTable("GetParametersFromSP",CommandType.StoredProcedure, queryParams, ConnStr, null);

                    listparams = dt.AsEnumerable().Select(x => new Tb_Parameters() {ParameterName = x.Field<string>("name"), DataType = x.Field<string>("name"), Length = x.Field<int>("max_length"), Presition=x.Field<int>("precision"), Nullable = x.Field<bool>("is_nullable"), IsOut = x.Field<bool>("is_output") }).ToList();

                    for(var i=0; i<listparams.Count; i++) 
                    {
                        listparams[i].ID_Object = tb_Objects.ID_Object;
                    }
                    db.Tb_Parameters.AddRange(listparams);
                    db.SaveChanges();

                    //Llamamos al metodo para almacenar los RS y las RSC
                    GetResultSet(tb_Objects.AuxNameObject, ConnStr);
                    return RedirectToAction("Index", new {idContext = idContext, idProject = idProject});
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName", tb_Objects.ID_Context);
            return View(tb_Objects);
        }

        // GET: Object/Edit/5
        public ActionResult Edit(int? id,int idContext, int idProject)
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
            ViewBag.idProject = idProject;
            ViewBag.idContext = idContext;
            ViewBag.CxtName = tb_Objects.Tb_Contexts.ContextName;
            return View(tb_Objects);
        }

        // POST: Object/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Object,ID_Context,ObjectName,Entity,ObjDescription,ID_AuxObject")] Tb_Objects tb_Objects, int idProject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(tb_Objects).State = EntityState.Modified;
                    db.SaveChanges();


                    //Eliminamos de la base de datos los parametros agregados con anterioridad
                    List<Tb_Parameters> tbParameters = db.Tb_Parameters.Where(x => x.ID_Object == tb_Objects.ID_Object).ToList();
                    if (tbParameters.Count>0)
                    {
                        db.Tb_Parameters.RemoveRange(tbParameters);
                        db.SaveChanges();
                    }

                    string ConnStr;
                    Tb_Projects tb = db.Tb_Projects.Find(idProject);
                    if (tb.LocalConnection.Value)
                        ConnStr = string.Format("data source=./;initial catalog={0};integrated security=True;MultipleActiveResultSets=True;App=EntityFramework", tb.ProjectDatabase);
                    else
                        ConnStr = string.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework", tb.Server, tb.ProjectDatabase, tb.ProjectUser, tb.Password); ;

                    List<SqlParameter> queryParams = new List<SqlParameter>();
                    queryParams.Add(new SqlParameter("idObject", tb_Objects.ID_AuxObject));

                    List<Tb_Parameters> listparams = new List<Tb_Parameters>();
                    DataTable dt = DataAccessADO.GetDataTable("GetParametersFromSP", CommandType.StoredProcedure, queryParams, ConnStr, null);

                    listparams = dt.AsEnumerable().Select(x => new Tb_Parameters() { ParameterName = x.Field<string>("name"), DataType = x.Field<string>("name"), Length = x.Field<int>("max_length"), Presition = x.Field<int>("precision"), Nullable = x.Field<bool>("is_nullable"), IsOut = x.Field<bool>("is_output") }).ToList();

                    for (var i = 0; i < listparams.Count; i++)
                    {
                        listparams[i].ID_Object = tb_Objects.ID_Object;
                    }
                    db.Tb_Parameters.AddRange(listparams);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { idContext = tb_Objects.ID_Context, idProject = idProject });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName", tb_Objects.ID_Context);
            return View(tb_Objects);
        }

        // GET: Object/Delete/5
        public ActionResult Delete(int? id, int idContext, int idProject)
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
            Tb_Contexts tb = db.Tb_Contexts.Find(idContext);
            ViewBag.CxtName = tb.ContextName;
            ViewBag.idProject = idProject;
            ViewBag.idContext = idContext;
            return View(tb_Objects);
        }

        // POST: Object/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int idContext, int idProject)
        {
            //List<Tb_Parameters> tbParameters = db.Tb_Parameters.Where(x => x.ID_Object == id).ToList();
            List<Tb_Parameters> tb_Parameters = new List<Tb_Parameters>();
            
            Tb_Objects tb_Objects = db.Tb_Objects.Find(id);
            if (tb_Objects.Tb_Parameters.Any())
            {
                tb_Parameters = db.Tb_Parameters.Where(x => x.ID_Object == id).ToList();
                db.Tb_Parameters.RemoveRange(tb_Parameters);
                db.SaveChanges();
            }

            db.Tb_Objects.Remove(tb_Objects);
            db.SaveChanges();
            return RedirectToAction("Index", new { idContext = idContext, idProject = idProject });
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
                ConnStr = string.Format("data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True;application name=EntityFramework", tb.Server, tb.ProjectDatabase, tb.ProjectUser, tb.Password);

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
