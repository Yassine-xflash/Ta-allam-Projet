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

            var aboutPage = new AboutPage
            {
                FormationsCount = db.Formations.Count(),
                ParticipantsCount = db.Participants.Count(),
                FormateursCount = db.Formateurs.Count(),
                AverageRating = db.FormationParticipants
            .Where(fp => fp.Rating > 0)
            .Average(fp => (double?)fp.Rating) ?? 0.0,
                PopularFormations = db.Formations
            .OrderByDescending(f => f.FormationParticipants.Count())
            .Take(3)
            .ToList(),
                RecentReviews = db.FormationParticipants
            .Where(fp => !string.IsNullOrEmpty(fp.Comment))
            .OrderByDescending(fp => fp.FormationParticipantID)
            .Take(5)
            .Select(fp => new ReviewViewModel
            {
                ParticipantName = fp.Participant.Nom + " " + fp.Participant.Prenom,
                Comment = fp.Comment,
                Rating = fp.Rating
            })
            .ToList() ?? new List<ReviewViewModel>()
            };

            return View(aboutPage);
        }
    }
}