using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Gestion_Centre_Formations.Controllers
{
    public class FormateurProfileController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: FormateurProfile
        public ActionResult Index()
        {
            if (Session["UserType"] == null) {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Formateur") {
            return RedirectToAction("Index", "Home");
            }

            var formateurId = int.Parse(Session["UserId"].ToString());
            // Récupérer les formations créées par le formateur
            var formations = db.Formations
                .Where(f => f.FormateurID == formateurId)
                .Include(f => f.Formateur)
                .ToList();

            // Préparer les données pour la vue
            var viewModel = new FormateurProfileViewModel
            {
                Formateur = db.Formateurs.Find(formateurId),
                Formations = formations
            };

            return View(viewModel);
        }




        // GET: FormateurProfile/Create
        public ActionResult Create()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Formateur")
            {
                return RedirectToAction("Index", "Home");
            }
            // Récupérer la liste des formateurs pour le dropdown
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Nom");
            return View();
        }

        // POST: FormateurProfile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FormationID,Titre,Description,Categorie,Duration,Prix")] Formation formation)
        {
            // Assume you have a way to get the logged-in Formateur's ID (e.g., from session, claims, etc.)
            int? loggedInFormateurId = int.Parse(Session["UserId"].ToString()); // Replace with your method to fetch the ID.

            if (loggedInFormateurId == null)
            {
                // If the ID cannot be retrieved, deny access
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "You must be logged in as a Formateur to create a formation.");
            }

            // Set the FormateurID for the Formation
            formation.FormateurID = loggedInFormateurId.Value;

            if (ModelState.IsValid)
            {
                db.Formations.Add(formation);
                db.SaveChanges();
                return RedirectToAction("Index", new { formateurId = formation.FormateurID });
            }

            // If invalid, return to the view
            return View(formation);
        }




        // GET: FormateurProfile/Edit/5
        public ActionResult Edit()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Formateur")
            {
                return RedirectToAction("Index", "Home");
            }

            var id = int.Parse(Session["UserId"].ToString());
            var formation = db.Formations.FirstOrDefault(f => f.FormateurID == id);

            if (formation == null)
            {
                return HttpNotFound();
            }

            // Populate the dropdown for Formateur selection
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Nom", formation.FormateurID);
            return View(formation);
        }


        // POST: FormateurProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FormationID,Titre,Description,Categorie,Duration,Prix,FormateurID")] Formation formation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { formateurId = formation.FormateurID });
            }

            // Récupérer la liste des formateurs pour le dropdown
            ViewBag.FormateurID = new SelectList(db.Formateurs, "FormateurID", "Nom", formation.FormateurID);
            return View(formation);
        }

        // POST: FormateurProfile/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var formation = db.Formations.Find(id);
            db.Formations.Remove(formation);
            db.SaveChanges();
            return RedirectToAction("Index", new { formateurId = formation.FormateurID });
        }

        public ActionResult Details(int formationId)
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Formateur")
            {
                return RedirectToAction("Index", "Home");
            }
            // Récupérer la formation et inclure les données nécessaires
            var formation = db.Formations
                .Include(f => f.Formateur)
                .Include(f => f.FormationParticipants.Select(fp => fp.Participant)) // Include Participants
                .FirstOrDefault(f => f.FormationID == formationId);

            if (formation == null)
            {
                return HttpNotFound();
            }

            // Extraire la liste des participants depuis FormationParticipants
            var participants = formation.FormationParticipants
                .Select(fp => fp.Participant)
                .ToList();

            // Calculer le prix total
            double totalPrice = formation.Prix * participants.Count;

            // Préparer les données pour la vue
            var viewModel = new FormateurFormationDetailsViewModel
            {
                Formation = formation,
                Participants = participants,
                TotalPrice = totalPrice
            };

            return View(viewModel);
        }



    }


}