using DinkToPdf;
using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Helpers;
using Gestion_Centre_Formations.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class CertificateController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Certificate/Certifications
        public ActionResult Certifications()
        {
            // Check if the user is logged in
            if (Session["UserType"] == null)
            {
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                return RedirectToAction("Login", "Authentification");
            }

            // Ensure only participants can access this page
            if (Session["UserType"].ToString() != "Participant")
            {
                TempData["ErrorMessage"] = "You are not authorized to access this page.";
                return RedirectToAction("Index", "Home");
            }

            // Get the participant ID from the session
            int participantId = int.Parse(Session["UserId"].ToString());

            // Retrieve courses with certificates for the participant
            var coursesWithCertificates = db.Formations
                .Where(f => f.FormationParticipants.Any(p => p.ParticipantID == participantId))
                .Select(f => new CertificateView
                {
                    FormationID = f.FormationID,
                    CourseTitle = f.Titre,
                    Certificate = db.Certificates.FirstOrDefault(c => c.FormationID == f.FormationID && c.ParticipantID == participantId),
                })
                .ToList();

            // Add the hash to each certificate
            foreach (var certificateView in coursesWithCertificates)
            {
                if (certificateView.Certificate != null)
                {
                    certificateView.CertificateHash = GenerateHash(certificateView.Certificate.CertificateID.ToString());
                }
            }

            return View(coursesWithCertificates);
        }
        // GET: Certificate/ViewCertificate
        public ActionResult ViewCertificate(string hashId)
        {
            // Check if the user is logged in
            if (Session["UserType"] == null)
            {
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                return RedirectToAction("Login", "Authentification");
            }

            // Ensure only participants can access this page
            if (Session["UserType"].ToString() != "Participant")
            {
                TempData["ErrorMessage"] = "You are not authorized to access this page.";
                return RedirectToAction("Index", "Home");
            }

            // Get the participant ID from the session
            int participantId = int.Parse(Session["UserId"].ToString());

            // Log the hash value to check if the URL is correct
            System.Diagnostics.Debug.WriteLine($"Received Hash: {hashId}");

            // Find the certificate by hashed ID
            var certificate = db.Certificates
                 .ToList() // Pull all certificates into memory (inefficient for large data sets)
                .FirstOrDefault(c => GenerateHash(c.CertificateID.ToString()) == hashId);

            // Check if the certificate exists
            if (certificate == null)
            {
                TempData["ErrorMessage"] = "Certificate not found.";
                return RedirectToAction("Index", "Home");
            }

            // Ensure the certificate belongs to the logged-in participant
            if (certificate.ParticipantID != participantId)
            {
                TempData["ErrorMessage"] = "You are not authorized to access this certificate.";
                return RedirectToAction("Index", "Home");
            }

            // Log certificate details for debugging purposes
            System.Diagnostics.Debug.WriteLine($"Certificate found: {certificate.CertificateID}");

            return View("ViewCertificate", certificate);
        }

        // Helper: Generate a hashed ID
        private string GenerateHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash)
                             .Replace("+", "")
                             .Replace("/", "")
                             .Replace("=", "") // Remove invalid URL characters
                             .Substring(0, 12); // Truncate to 12 characters for simplicity
            }
        }

        // Properly dispose of the database context
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
