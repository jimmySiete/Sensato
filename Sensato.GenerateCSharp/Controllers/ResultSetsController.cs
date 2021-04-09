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
    public class ResultSetsController : Controller
    {
        private DB_GeneratorEntities db = new DB_GeneratorEntities();

        // GET: ResultSets
        public ActionResult Index(int idObject, int idContext, int idProject)
        {
            Tb_Objects tbO = db.Tb_Objects.Find(idObject);
            ViewBag.ObjName = tbO.ObjectName;
            ViewBag.idObject = idObject;
            ViewBag.idContext = idContext;
            ViewBag.idProject = idProject;
            var tb_ResultSets = db.Tb_ResultSets.Include(t => t.Tb_Objects);
            return View(tb_ResultSets.Where(x=>x.ID_Object == idObject).ToList());
        }

        // GET: ResultSets/Edit/5
        public ActionResult Edit(int? id, int idObject, int idContext)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_ResultSets tb_ResultSets = db.Tb_ResultSets.Find(id);
            if (tb_ResultSets == null)
            {
                return HttpNotFound();
            }
            ViewBag.idObject = idObject;
            ViewBag.idContext = idContext;
            ViewBag.objName = db.Tb_Objects.Where(x => x.ID_Object == idObject).Select(x => x.ObjectName);
            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName", tb_ResultSets.ID_Object);
            return View(tb_ResultSets);
        }

        // POST: ResultSets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ResultSet,ID_Object,ResultSetName")] Tb_ResultSets tb_ResultSets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_ResultSets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Object = new SelectList(db.Tb_Objects, "ID_Object", "ObjectName", tb_ResultSets.ID_Object);
            return View(tb_ResultSets);
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
