using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Gestion_Centre_Formations.Controllers
{
    public class HomeController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();
        // GET: Home
        public ActionResult Index(string searchString, string category, double? minPrice, double? maxPrice, int? minDuration, int? maxDuration)
        {
            // Start with base query
            var formations = db.Formations.Include(f => f.Formateur).AsQueryable();

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
            ViewBag.Categories = db.Formations.Select(f => f.Categorie).Distinct().ToList();

            // Get price and duration ranges
            ViewBag.MinPrice = db.Formations.Min(f => f.Prix);
            ViewBag.MaxPrice = db.Formations.Max(f => f.Prix);
            ViewBag.MinDuration = db.Formations.Min(f => f.Duration);
            ViewBag.MaxDuration = db.Formations.Max(f => f.Duration);

            return View(formations.ToList());
        }
    }



}
