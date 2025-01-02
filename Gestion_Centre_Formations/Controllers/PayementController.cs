using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class PayementController : Controller
    {
        // GET: Payement
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPayment(PaymentModel model)
        {
            if (ModelState.IsValid)
            {
                // Process payment logic
                return RedirectToAction("Success");
            }
            return View(model);
        }

    }
}