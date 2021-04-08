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
        public ActionResult Index(int idObject)
        {
            ViewBag.idObject = idObject;
            Tb_Objects tbO = db.Tb_Objects.Find(idObject);
            ViewBag.ObjName = tbO.ObjectName;
            var tb_Parameters = db.Tb_Parameters.Include(t => t.Tb_Objects);
            return View(tb_Parameters.Where(x=>x.ID_Object == idObject).ToList());
        }

        // GET: Parameters/Create
        public ActionResult Create()
        {
            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName");
            return View();
        }

        // POST: Parameters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Parameter,ID_Object,ParameterName,DataType,Length,Presition,Nullable,IsOut")] Tb_Parameters tb_Parameters)
        {
            if (ModelState.IsValid)
            {
                db.Tb_Parameters.Add(tb_Parameters);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName", tb_Parameters.ID_Object);
            return View(tb_Parameters);
        }

        // GET: Parameters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Parameters tb_Parameters = db.Tb_Parameters.Find(id);
            if (tb_Parameters == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName", tb_Parameters.ID_Object);
            return View(tb_Parameters);
        }

        // POST: Parameters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Parameter,ID_Object,ParameterName,DataType,Length,Presition,Nullable,IsOut")] Tb_Parameters tb_Parameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Parameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName", tb_Parameters.ID_Object);
            return View(tb_Parameters);
        }

        // GET: Parameters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_Parameters tb_Parameters = db.Tb_Parameters.Find(id);
            if (tb_Parameters == null)
            {
                return HttpNotFound();
            }
            return View(tb_Parameters);
        }

        // POST: Parameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tb_Parameters tb_Parameters = db.Tb_Parameters.Find(id);
            db.Tb_Parameters.Remove(tb_Parameters);
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
