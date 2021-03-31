using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using Sensato.GenerateCSharp.Models;

namespace Sensato.GenerateCSharp.Controllers
{
    public class ObjectController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: Object
        public ActionResult Index(int idContext)
        {
            ViewBag.idContext = idContext;
            Tb_Contexts tbC = db.Tb_Contexts.Find(idContext);
            ViewBag.CxtName = tbC.ContextName;
            ViewBag.idProject = tbC.Tb_Projects.ID_Project;
            var tb_Objects = db.Tb_Objects.Include(t => t.Tb_Contexts);
            return View(tb_Objects.Where(x => x.ID_Context == idContext).ToList());
        }

        // GET: Object/Create
        public ActionResult Create(int idContext)
        {
            ViewBag.idContext = idContext;
            Tb_Contexts tb = db.Tb_Contexts.Find(idContext);
            ViewBag.CxtName = tb.ContextName;
            ViewBag.idProject = tb.ID_Project;
            //ViewBag.ID_Context = new SelectList(db.Tb_Contexts, "ID_Context", "ContextName");
            return View();
        }

        // POST: Object/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Object,ID_Context,ObjectName")] Tb_Objects tb_Objects, int idContext)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Tb_Objects.Add(tb_Objects);

                        db.SaveChanges();
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
