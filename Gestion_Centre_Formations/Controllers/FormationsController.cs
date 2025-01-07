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
        public ActionResult Index(string searchString, string category, double? minPrice, double? maxPrice, int? minDuration, int? maxDuration)
        {
            // Start with base query
            var formations = db.Formations.Include(f => f.Formateur)
                                          .Where(f => f.Supp == false) // Filter out formations marked for deletion
                                          .AsQueryable();

            // Search by title
            if (!string.IsNullOrEmpty(searchString))
            {
                formations = formations.Where(f => f.Titre.Contains(searchString));
            }

            // Filter by category
            if (!string.IsNullOrEmpty(category))
            {
                formations = formations.Where(f => f.Categorie == category);
            }

            // Filter by price range
            if (minPrice.HasValue)
            {
                formations = formations.Where(f => f.Prix >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                formations = formations.Where(f => f.Prix <= maxPrice.Value);
            }

            // Filter by duration range
            if (minDuration.HasValue)
            {
                formations = formations.Where(f => f.Duration >= minDuration.Value);
            }
            if (maxDuration.HasValue)
            {
                formations = formations.Where(f => f.Duration <= maxDuration.Value);
            }

            // Get unique categories for dropdown
            ViewBag.Categories = db.Formations.Where(f => f.Supp == false) // Only include non-deleted formations
                                              .Select(f => f.Categorie)
                                              .Distinct()
                                              .ToList();

            // Get price and duration ranges for filter inputs
            ViewBag.MinPrice = db.Formations.Where(f => f.Supp == false).Min(f => f.Prix);
            ViewBag.MaxPrice = db.Formations.Where(f => f.Supp == false).Max(f => f.Prix);
            ViewBag.MinDuration = db.Formations.Where(f => f.Supp == false).Min(f => f.Duration);
            ViewBag.MaxDuration = db.Formations.Where(f => f.Supp == false).Max(f => f.Duration);

            // Pass current filter values to the view
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;
            ViewBag.MinPriceFilter = minPrice;
            ViewBag.MaxPriceFilter = maxPrice;
            ViewBag.MinDurationFilter = minDuration;
            ViewBag.MaxDurationFilter = maxDuration;

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
        public ActionResult Join(int courseId)
        {
            // Check if the user is logged in by verifying the session
            if (Session["UserId"] != null)
            {
                // Get the logged-in user's ID from the session
                int userId = Convert.ToInt32(Session["UserId"]);

                // Redirect to the Payment action with both courseId and userId
                return RedirectToAction("Index", "Payement", new { courseId, userId });
            }

            // If the user is not logged in, redirect to the Login page
            return RedirectToAction("Login", "Authentification");
        }
    }
}
