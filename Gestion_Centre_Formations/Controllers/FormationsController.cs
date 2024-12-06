using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;

namespace Gestion_Centre_Formations.Controllers
{
    public class FormationsController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Formations
        public ActionResult Index()
        {
            var formations = db.Formations.Include(f => f.Formateur);
            return View(formations.ToList());
        }

        // GET: Formations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formation formation = db.Formations.Find(id);
            if (formation == null)
            {
                return HttpNotFound();
            }
            return View(formation);
        }

        // GET: Formations/Create
        public ActionResult Create()
        {
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Specialisation");
            return View();
        }

        // POST: Formations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormationID,Titre,Description,Categorie,Duration,Prix,FormateurID")] Formation formation)
        {
            if (ModelState.IsValid)
            {
                db.Formations.Add(formation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Specialisation", formation.FormateurID);
            return View(formation);
        }

        // GET: Formations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formation formation = db.Formations.Find(id);
            if (formation == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Specialisation", formation.FormateurID);
            return View(formation);
        }

        // POST: Formations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormationID,Titre,Description,Categorie,Duration,Prix,FormateurID")] Formation formation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Specialisation", formation.FormateurID);
            return View(formation);
        }

        // GET: Formations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formation formation = db.Formations.Find(id);
            if (formation == null)
            {
                return HttpNotFound();
            }
            return View(formation);
        }

        // POST: Formations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Formation formation = db.Formations.Find(id);
            db.Formations.Remove(formation);
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
