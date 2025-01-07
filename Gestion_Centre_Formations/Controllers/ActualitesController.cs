using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class ActualitesController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();
        private const int PageSize = 5; 

        // GET: Actualites
        public ActionResult Index(int page = 1)
        {
            var actualitesQuery = db.Actualites
                .Where(a => !a.Supp)
                .OrderByDescending(a => a.Date); 

            int totalActualites = actualitesQuery.Count();

            int totalPages = (int)Math.Ceiling((double)totalActualites / PageSize);

            var actualites = actualitesQuery
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            var mostPopularActualites = db.Actualites
                .Where(a => !a.Supp)
                .OrderByDescending(a => a.Views)
                .Take(5) 
                .ToList();

            ViewBag.MostPopularActualites = mostPopularActualites;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(actualites);
        }

        // GET: Actualites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var actualite = db.Actualites
                .Include(a => a.Sections)
                .SingleOrDefault(a => a.Id == id);

            if (actualite == null || actualite.Supp)
            {
                return HttpNotFound();
            }

            actualite.Views++;
            db.Entry(actualite).State = EntityState.Modified;
            db.SaveChanges();

            var mostPopularActualites = db.Actualites
                .Where(a => !a.Supp)
                .OrderByDescending(a => a.Views)
                .Take(5) 
                .ToList();

            ViewBag.MostPopularActualites = mostPopularActualites;

            return View(actualite);
        }
    }
}