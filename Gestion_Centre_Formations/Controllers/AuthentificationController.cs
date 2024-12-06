using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class AuthentificationController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Account/SignUp
        public ActionResult SignUp()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Account/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Hash the password
                    string hashedPassword = HashPassword(model.Password);

                    if (model.UserType == "Participant")
                    {
                        var participant = new Participant
                        {
                            Nom = model.Nom,
                            Prenom = model.Prenom,
                            Email = model.Email,
                            NumTel = model.NumTel,
                            Password = hashedPassword,
                            DateInscription = DateTime.Now,
                            NbrFormations = 0
                        };
                        db.Participants.Add(participant);
                    }
                    else if (model.UserType == "Formateur")
                    {
                        var formateur = new Formateur
                        {
                            Nom = model.Nom,
                            Prenom = model.Prenom,
                            Email = model.Email,
                            NumTel = model.NumTel,
                            Password = hashedPassword,
                            Specialisation = model.Specialisation,
                            NbrFormations = 0
                        };
                        db.Formateurs.Add(formateur);
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Registration Successful! Please Log In.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred during registration: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Authentification/Login
        public ActionResult Login()
        {
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Authentification/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogInModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Hash the input password
                    string hashedPassword = HashPassword(model.Password);

                    // Check if user is a participant
                    var participant = db.Participants
                        .FirstOrDefault(p => p.Email == model.Email && p.Password == hashedPassword);

                    // Check if user is a formateur
                    var formateur = db.Formateurs
                        .FirstOrDefault(f => f.Email == model.Email && f.Password == hashedPassword);// Check if user is a formateur
                    
                    //check if user is admin
                    var admin = db.Admins
                        .FirstOrDefault(a => a.Email == model.Email && a.Password == hashedPassword);

                    if (participant != null)
                    {
                        // Set session for participant
                        Session["UserId"] = participant.ParticipantID;
                        Session["UserType"] = "Participant";
                        Session["Nom"] = participant.Nom;
                        Session["Prenom"] = participant.Prenom;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (formateur != null)
                    {
                        // Set session for formateur
                        Session["UserId"] = formateur.FormateurID;
                        Session["UserType"] = "Formateur";
                        Session["Nom"] = formateur.Nom;
                        Session["Prenom"] = formateur.Prenom;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (admin != null)
                    {
                        // Set session for formateur
                        Session["UserId"] = admin.AdminID;
                        Session["UserType"] = "Admin";
                        Session["Nom"] = admin.Nom;
                        Session["Prenom"] = admin.Prenom;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred during login: " + ex.Message);
                    return View(model);
                }
            }
            return View(model);
        }

        // Logout Action
        public ActionResult Logout()
        {
            // Clear all session variables
            Session.Clear();
            return RedirectToAction("Login");
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
                return builder.ToString();
            }
        }

    }
}