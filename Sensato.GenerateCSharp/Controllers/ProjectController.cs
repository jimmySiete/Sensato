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
    public class ProjectController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: Project
        public ActionResult Index()
        {
            return View(db.Tb_Projects.ToList());
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Project,ProjectName,FileDirectory,Server,ProjectUser,ProjectDatabase,Password,CreationDate")] Tb_Projects tb_Projects)
        {
            if (ModelState.IsValid)
            {
                db.Tb_Projects.Add(tb_Projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_Projects);
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Projects tb_Projects = db.Tb_Projects.Find(id);
            if (tb_Projects == null)
            {
                return HttpNotFound();
            }
            return View(tb_Projects);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Project,ProjectName,FileDirectory,Server,ProjectUser,ProjectDatabase,Password,CreationDate")] Tb_Projects tb_Projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tb_Projects);
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Projects tb_Projects = db.Tb_Projects.Find(id);
            if (tb_Projects == null)
            {
                return HttpNotFound();
            }
            return View(tb_Projects);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Projects tb_Projects = db.Tb_Projects.Find(id);
            db.Tb_Projects.Remove(tb_Projects);
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
