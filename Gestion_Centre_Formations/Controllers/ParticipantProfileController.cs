using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class ParticipantProfileController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        public ActionResult Index()
        {
            if (Session["UserType"] == null)
            {
                return RedirectToAction("Login", "Authentification");
            }
            else if (Session["UserType"].ToString() != "Participant")
            {
                // return RedirectToAction("Index", "Home");
            }

            var participantId = int.Parse(Session["UserId"].ToString());
            var enrolledCourses = db.FormationParticipants
                .Where(fp => fp.Participant.ParticipantID == participantId)
                .Include(fp => fp.Formation)
                .Include(fp => fp.Formation.Formateur)
                .Include(fp => fp.Participant)
                .ToList();

            return View(enrolledCourses);
        }

        public ActionResult Rate(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var enrollment = db.FormationParticipants
                    .Include(fp => fp.Formation)
                    .FirstOrDefault(fp => fp.FormationParticipantID == id);

                if (enrollment == null)
                {
                    return HttpNotFound();
                }

                return View(enrollment);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return View("Error", new HandleErrorInfo(ex, "ParticipantProfile", "Rate"));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRating(int formationParticipantId, int rating, string comment)
        {
            try
            {
                var enrollment = db.FormationParticipants.Find(formationParticipantId);

                if (enrollment != null)
                {
                    enrollment.Rating = rating;
                    enrollment.Comment = comment;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here
                return View("Error", new HandleErrorInfo(ex, "ParticipantProfile", "SubmitRating"));
            }
        }

        public ActionResult RemoveRating(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enrollment = db.FormationParticipants.Find(id);
            if (enrollment != null)
            {
                enrollment.Rating = 0; 
                enrollment.Comment = null;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerateCertificate(int participantId, int formationId)
        {
            // Check if the participant and formation exist
            var participant = db.Participants.Find(participantId);
            var formation = db.Formations.Find(formationId);

            if (participant == null || formation == null)
            {
                return HttpNotFound();
            }

            // Check if the participant has completed the course
            var enrollment = db.FormationParticipants
                .FirstOrDefault(fp => fp.ParticipantID == participantId && fp.FormationID == formationId);

            if (enrollment == null || !enrollment.Formation.Completed)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Course not completed.");
            }

            // Check if the participant already has a certificate for this course
            var existingCertificate = db.Certificates
                .FirstOrDefault(c => c.ParticipantID == participantId && c.FormationID == formationId);

            if (existingCertificate != null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Certificate already issued.");
            }

            // Create a new certificate
            var certificate = new Certificate
            {
                Title = $"Certificate of Completion for {formation.Titre}",
                DateIssued = DateTime.Now,
                Supp = false,
                ParticipantID = participantId,
                FormationID = formationId
            };

            // Add the certificate to the database
            db.Certificates.Add(certificate);
            db.SaveChanges();

            // Update the enrollment to mark that the certificate has been issued
            enrollment.HasCertificate = true;
            db.SaveChanges();

            // Redirect to a success page or return a success message
            return RedirectToAction("Index", "Certificate");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}