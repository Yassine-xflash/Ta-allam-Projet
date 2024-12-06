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
    public class FormationParticipantsController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: FormationParticipants
        public ActionResult Index()
        {
            var formationParticipants = db.FormationParticipants.Include(f => f.Formation).Include(f => f.Participant);
            return View(formationParticipants.ToList());
        }

        // GET: FormationParticipants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormationParticipant formationParticipant = db.FormationParticipants.Find(id);
            if (formationParticipant == null)
            {
                return HttpNotFound();
            }
            return View(formationParticipant);
        }

        // GET: FormationParticipants/Create
        public ActionResult Create()
        {
            ViewBag.FormationID = new SelectList(db.Formations, "FormationID", "Titre");
            ViewBag.ParticipantID = new SelectList(db.Participants, "ParticipantID", "Nom");
            return View();
        }

        // POST: FormationParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormationParticipantID,FormationID,ParticipantID")] FormationParticipant formationParticipant)
        {
            if (ModelState.IsValid)
            {
                db.FormationParticipants.Add(formationParticipant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FormationID = new SelectList(db.Formations, "FormationID", "Titre", formationParticipant.FormationID);
            ViewBag.ParticipantID = new SelectList(db.Participants, "ParticipantID", "Nom", formationParticipant.ParticipantID);
            return View(formationParticipant);
        }

        // GET: FormationParticipants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormationParticipant formationParticipant = db.FormationParticipants.Find(id);
            if (formationParticipant == null)
            {
                return HttpNotFound();
            }
            ViewBag.FormationID = new SelectList(db.Formations, "FormationID", "Titre", formationParticipant.FormationID);
            ViewBag.ParticipantID = new SelectList(db.Participants, "ParticipantID", "Nom", formationParticipant.ParticipantID);
            return View(formationParticipant);
        }

        // POST: FormationParticipants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormationParticipantID,FormationID,ParticipantID")] FormationParticipant formationParticipant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formationParticipant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FormationID = new SelectList(db.Formations, "FormationID", "Titre", formationParticipant.FormationID);
            ViewBag.ParticipantID = new SelectList(db.Participants, "ParticipantID", "Nom", formationParticipant.ParticipantID);
            return View(formationParticipant);
        }

        // GET: FormationParticipants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormationParticipant formationParticipant = db.FormationParticipants.Find(id);
            if (formationParticipant == null)
            {
                return HttpNotFound();
            }
            return View(formationParticipant);
        }

        // POST: FormationParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormationParticipant formationParticipant = db.FormationParticipants.Find(id);
            db.FormationParticipants.Remove(formationParticipant);
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
