using Gestion_Centre_Formations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class TestController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();
        // GET: Test
        public ActionResult SeedTestData()
        {
            try
            {
                db.SeedDatabase();
                return Content("Database seeded successfully with test data!");
            }
            catch (Exception ex)
            {
                return Content($"Error seeding database: {ex.Message}");
            }
        }
    }
}