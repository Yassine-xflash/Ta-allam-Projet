using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class ProfileController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Profile
        public ActionResult Index()
        {
            var userId = Session["UserId"];

            if (userId == null)
            {
                return RedirectToAction("Login", "Authentification");
            }

            int participantId;
            if (!int.TryParse(userId.ToString(), out participantId))
            {
                return RedirectToAction("Login", "Authentification");
            }

            var participant = db.Participants.FirstOrDefault(p => p.ParticipantID == participantId);

            if (participant == null)
            {
                return HttpNotFound();
            }

            var formations = db.FormationParticipants
                .Where(fp => fp.ParticipantID == participantId)
                .Include(fp => fp.Formation)
                .Select(fp => fp.Formation)
                .ToList();

            // Debugging: Print participant ID and formations
            System.Diagnostics.Debug.WriteLine($"Participant ID: {participantId}");
            foreach (var formation in formations)
            {
                System.Diagnostics.Debug.WriteLine($"Formation ID: {formation.FormationID}, Title: {formation.Titre}");
            }

            var viewModel = new ParticipantProfileViewModel
            {
                Participant = participant, // Add the Participant object to the view model
                Formations = formations
            };

            return View(viewModel);
        }






        // GET: Profile/Edit
        public ActionResult Edit()
        {
            var userId = Session["UserId"];

            if (userId == null)
            {
                return RedirectToAction("Login", "Authentification");
            }

            int participantId;
            if (!int.TryParse(userId.ToString(), out participantId))
            {
                return RedirectToAction("Login", "Authentification");
            }

            var participant = db.Participants.FirstOrDefault(p => p.ParticipantID == participantId);

            if (participant == null)
            {
                return HttpNotFound();
            }

            participant.Password = "";

            return View(participant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Participant participant)
        {
            // Debugging: Log the start of the Edit action
            System.Diagnostics.Debug.WriteLine("Edit action started.");

            if (ModelState.IsValid)
            {
                // Debugging: Log that the model state is valid
                System.Diagnostics.Debug.WriteLine("ModelState is valid.");

                // Fetch the existing participant from the database
                var existingParticipant = db.Participants.Find(participant.ParticipantID);

                if (existingParticipant == null)
                {
                    // Debugging: Log that the participant was not found
                    System.Diagnostics.Debug.WriteLine("Participant not found in the database.");
                    return HttpNotFound();
                }

                // Debugging: Log the participant details before updating
                System.Diagnostics.Debug.WriteLine("Existing Participant Details:");
                System.Diagnostics.Debug.WriteLine($"ID: {existingParticipant.ParticipantID}, Prenom: {existingParticipant.Prenom}, Nom: {existingParticipant.Nom}, Email: {existingParticipant.Email}, NumTel: {existingParticipant.NumTel}");

                // Update only the editable fields
                existingParticipant.Prenom = participant.Prenom;
                existingParticipant.Nom = participant.Nom;
                existingParticipant.Email = participant.Email;
                existingParticipant.NumTel = participant.NumTel;
                existingParticipant.Password = HashPassword(participant.Password);

                // Debugging: Log the participant details after updating
                System.Diagnostics.Debug.WriteLine("Updated Participant Details:");
                System.Diagnostics.Debug.WriteLine($"ID: {existingParticipant.ParticipantID}, Prenom: {existingParticipant.Prenom}, Nom: {existingParticipant.Nom}, Email: {existingParticipant.Email}, NumTel: {existingParticipant.NumTel}");

                // Save changes to the database
                db.SaveChanges();

                // Debugging: Log that changes were saved successfully
                System.Diagnostics.Debug.WriteLine("Changes saved to the database.");

                // Redirect to the Index action to display the updated profile
                return RedirectToAction("Index");
            }
            else
            {
                // Debugging: Log that the model state is invalid
                System.Diagnostics.Debug.WriteLine("ModelState is invalid.");

                // Debugging: Log all validation errors
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Validation Error for {key}: {error.ErrorMessage}");
                    }
                }
            }

            // If the model state is invalid, return to the Edit view with validation errors
            return View(participant);
        }

        // Password Hashing Method
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                Console.WriteLine("123456");
                return builder.ToString();
            }

        }
    }
}
