using Gestion_Centre_Formations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Gestion_Centre_Formations.Controllers
{
    public class BaseController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Calculate the number of pending payments
            var pendingPaymentsCount = db.Payments
                .Count(p => p.PaymentStatus == "Pending Approval");

            // Store it in ViewBag
            ViewBag.PendingPaymentsCount = pendingPaymentsCount;
        }
    }
}