using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class AProposController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();
        // GET: APropos
        public ActionResult Index()
        {

            var FormationsCount = db.Formations.Count();
            var participantsCount = db.Participants.Count();
            var FormateursCiunt = db.Formateurs.Count();
            var averageRating = 4.8;

            // Récupérer les formations les plus populaires
            var popularFormations = db.Formations
                .OrderByDescending(f => f.FormationParticipants.Count())
                .Take(3)
                .ToList();

            // Préparer les données pour la vue
            var viewModel = new AboutPage
            {
                FormationsCount = FormationsCount,
                ParticipantsCount = participantsCount,
                AverageRating = averageRating,
                PopularFormations = popularFormations
            };

            return View(viewModel);
        }
    }
}