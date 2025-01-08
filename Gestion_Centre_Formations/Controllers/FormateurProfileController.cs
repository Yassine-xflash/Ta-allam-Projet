using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.IO;

namespace Gestion_Centre_Formations.Controllers
{
    public class FormateurProfileController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: FormateurProfile
        public ActionResult Index()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Formateur")
            {
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
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Formateur")
            {
                return RedirectToAction("Login", "Authentification");
            }

            // No need to fetch formateurs since it's already tied to the logged-in formateur
            return View();
        }

        // POST: FormateurProfile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormationsPage model)
        {
            int? loggedInFormateurId = int.Parse(Session["UserId"].ToString());
            if (loggedInFormateurId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "You must be logged in as a Formateur to create a formation.");
            }

            if (ModelState.IsValid)
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    var fileName = model.Titre + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/assets/images/course"), fileName);
                    model.ImageFile.SaveAs(path);
                }
                else
                {
                    Console.WriteLine("No image saved'");
                }


                var formation = new Formation
                {
                    Titre = model.Titre,
                    Categorie = model.Categorie,
                    Description = model.Description,
                    FormateurID = loggedInFormateurId.Value,
                    Prix = model.Prix,
                    Duration = model.Duration,
                    Supp = false
                };

                db.Formations.Add(formation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }





        // GET: FormateurProfile/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["UserType"] == null || Session["UserType"].ToString() != "Formateur")
            {
                return RedirectToAction("Login", "Authentification");
            }

            int formateurId = int.Parse(Session["UserId"].ToString());
            var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && f.FormateurID == formateurId);
            if (formation == null)
            {
                return HttpNotFound();
            }

            var model = new FormationsPage
            {
                Titre = formation.Titre,
                Description = formation.Description,
                Categorie = formation.Categorie,
                Duration = formation.Duration,
                Prix = formation.Prix,
                FormateurID = formation.FormateurID
            };

            return View(model);
        }

        // POST: FormateurProfile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormationsPage model)
        {
            int formateurId = int.Parse(Session["UserId"].ToString());
            if (ModelState.IsValid)
            {
                var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && f.FormateurID == formateurId);
                if (formation == null)
                {
                    return HttpNotFound();
                }

                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                    var fileName = model.Titre + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/assets/images/course"), fileName);
                    model.ImageFile.SaveAs(path);


                }

                formation.Titre = model.Titre;
                formation.Description = model.Description;
                formation.Categorie = model.Categorie;
                formation.Duration = model.Duration;
                formation.Prix = model.Prix;

                db.Entry(formation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
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

        public JsonResult GetFormationsParticipantsCount()
        {
            var formateurId = int.Parse(Session["UserId"].ToString());
            var formationsData = db.Formations
                .Where(f => f.FormateurID == formateurId)
                .Select(f => new
                {
                    f.Titre,
                    ParticipantsCount = f.FormationParticipants.Count()
                }).ToList();

            return Json(formationsData, JsonRequestBehavior.AllowGet);
        }




    }


}