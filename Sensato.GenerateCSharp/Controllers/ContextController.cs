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
using Sensato.GenerateCSharp.GlobalCode;

namespace Sensato.GenerateCSharp.Controllers
{
    public class ContextController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: Context
        public ActionResult Index(int ID_Project)
        {
            ViewBag.CurrentProject = ID_Project;
            Tb_Projects proj = db.Tb_Projects.Find(ID_Project);
            ViewBag.CurrentName = proj.ProjectName;
            var tb_Contexts = db.Tb_Contexts.Include(t => t.Tb_Projects);
            return View(tb_Contexts.Where(x=>x.ID_Project == ID_Project).ToList());
        }

        // GET: Context/Create
        public ActionResult Create(int ID_Project)
        {
            ViewBag.idProject = ID_Project;
            Tb_Projects tb = db.Tb_Projects.Find(ID_Project);
            ViewBag.CurrentName = tb.ProjectName;
            return View();
        }

        // POST: Context/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Context,ID_Project,ContextName,CreationDate")] Tb_Contexts tb_Contexts, int ID_Project)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Tb_Contexts.Add(tb_Contexts);
                        tb_Contexts.ID_Project = ID_Project;
                        tb_Contexts.CreationDate = DateTime.Now.Date;
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index",new { ID_Project = ID_Project });
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                    db.Tb_Contexts.Add(tb_Contexts);
                tb_Contexts.CreationDate = DateTime.Now.Date;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Project = new SelectList(db.Tb_Projects, "ID_Project", "ProjectName", tb_Contexts.ID_Project);
            return View(tb_Contexts);
        }

        // GET: Context/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Contexts tb_Contexts = db.Tb_Contexts.Find(id);
            if (tb_Contexts == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_Project = tb_Contexts.Tb_Projects.ID_Project;
            ViewBag.CurrentName = tb_Contexts.Tb_Projects.ProjectName;
            return View(tb_Contexts);
        }

        // POST: Context/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Context,ID_Project,ContextName,CreationDate")] Tb_Contexts tb_Contexts, int ID_Project)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(tb_Contexts).State = EntityState.Modified;
                        tb_Contexts.CreationDate = DateTime.Now.Date;
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index", new { ID_Project = ID_Project });
                    }
                    catch (Exception ex) 
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            ViewBag.ID_Project = new SelectList(db.Tb_Projects, "ID_Project", "ProjectName", tb_Contexts.ID_Project);
            return View(tb_Contexts);
        }

        // GET: Context/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Contexts tb_Contexts = db.Tb_Contexts.Find(id);
            ViewBag.CurrentName = tb_Contexts.Tb_Projects.ProjectName;
            ViewBag.id_Project = tb_Contexts.Tb_Projects.ID_Project;
            if (tb_Contexts == null)
            {
                return HttpNotFound();
            }
            return View(tb_Contexts);
        }

        // POST: Context/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Contexts tb_Contexts = db.Tb_Contexts.Find(id);
            if (ModelState.IsValid)
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try 
                    {
                        db.Tb_Contexts.Remove(tb_Contexts);
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index", new { ID_Project = tb_Contexts.ID_Project });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            return View(tb_Contexts);
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
