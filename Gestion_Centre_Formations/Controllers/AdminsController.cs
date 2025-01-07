using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Gestion_Centre_Formations.CustomAttributes;
using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;

namespace Gestion_Centre_Formations.Controllers
{
    [CustomAuthorize("Admin")]
    public class AdminsController : BaseController
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Admins
        public ActionResult Index()
        {
            var FormationsCount = db.Formations.Count();
            var participantsCount = db.Participants.Count();
            var FormateursCiunt = db.Formateurs.Count();
            var participants = db.Participants.ToList();
            var Certificates = db.Certificates.ToList();
            if (participants == null || participants.Count == 0)
            {
                throw new Exception("No participants found in the database.");
            }
            var Formations = db.Formations.ToList();
            if (Formations == null || Formations.Count == 0)
            {
                throw new Exception("No Formations found in the database.");
            }
            var viewModel = new AdminPage
            {
                FormationsCount = FormationsCount,
                ParticipantsCount = participantsCount,
                FormateursCount = FormateursCiunt,
                Participants = participants,
                Formations = Formations,
                Certificates = Certificates
            };
            return View(viewModel);
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminID,Nom,Prenom,Email,NumTel,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID,Nom,Prenom,Email,NumTel,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // === User CRUD ===

        public ActionResult Users()
        {
            // Retrieve only non-deleted users (Supp = false)
            var participants = db.Participants.Where(p => !p.Supp).ToList();
            var formateurs = db.Formateurs.Where(f => !f.Supp).ToList();
            var model = new AdminPage
            {
                Participants = participants,
                Formateurs = formateurs
            };
            return View(model);
        }

        public ActionResult ProfileUser(int id)
        {
            var participant = db.Participants.Find(id);
            if (participant != null && !participant.Supp)
            {
                return View(participant);
            }

            var formateur = db.Formateurs.Find(id);
            if (formateur != null && !formateur.Supp)
            {
                return View(formateur);
            }

            return HttpNotFound();
        }

        // GET: Admin/CreateUser
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(AdminPage model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserType == "Participant")
                {
                    var participant = new Participant
                    {
                        Nom = model.Nom,
                        Prenom = model.Prenom,
                        Email = model.Email,
                        NumTel = model.NumTel,
                        Password = HashPassword(model.Password),
                        DateInscription = DateTime.Now,
                        NbrFormations = 0,
                        Supp = false 
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
                        Password = HashPassword(model.Password),
                        Specialisation = model.Specialisation,
                        NbrFormations = 0, 
                        Supp = false 
                    };
                    db.Formateurs.Add(formateur);
                }

                db.SaveChanges();
                return RedirectToAction("Users");
            }

            return View(model);
        }

        // GET: Admin/EditUser/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var participant = db.Participants.Find(id);
            if (participant != null && !participant.Supp)
            {
                var model = new AdminPage
                {
                    Nom = participant.Nom,
                    Prenom = participant.Prenom,
                    Email = participant.Email,
                    NumTel = participant.NumTel,
                    Password = HashPassword(participant.Password),
                    ConfirmPassword = HashPassword(participant.Password),
                    UserType = "Participant"
                };
                return View(model);
            }

            var formateur = db.Formateurs.Find(id);
            if (formateur != null && !formateur.Supp)
            {
                var model = new AdminPage
                {
                    Nom = formateur.Nom,
                    Prenom = formateur.Prenom,
                    Email = formateur.Email,
                    NumTel = formateur.NumTel,
                    Password = HashPassword(formateur.Password),
                    ConfirmPassword = HashPassword(formateur.Password),
                    UserType = "Formateur"
                };
                return View(model);
            }

            return HttpNotFound();
        }

        // POST: Admins/EditUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, AdminPage model)
        {
            if (ModelState.IsValid)
            {
                if (model.UserType == "Participant")
                {
                    var participant = db.Participants.Find(id);
                    if (participant == null || participant.Supp)
                    {
                        return HttpNotFound();
                    }

                    participant.Nom = model.Nom;
                    participant.Prenom = model.Prenom;
                    participant.Email = model.Email;
                    participant.NumTel = model.NumTel;
                    participant.Password = HashPassword(model.Password);

                    db.Entry(participant).State = EntityState.Modified;
                }
                else if (model.UserType == "Formateur")
                {
                    var formateur = db.Formateurs.Find(id);
                    if (formateur == null || formateur.Supp)
                    {
                        return HttpNotFound();
                    }

                    formateur.Nom = model.Nom;
                    formateur.Prenom = model.Prenom;
                    formateur.Email = model.Email;
                    formateur.NumTel = model.NumTel;
                    formateur.Password = HashPassword(model.Password);
                    formateur.Specialisation = model.Specialisation;

                    db.Entry(formateur).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Users");
            }

            return View(model);
        }
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

        // GET: Admins/DeleteUser/5
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var participant = db.Participants.Find(id);
            if (participant != null && !participant.Supp)
            {
                return View(participant);
            }

            var formateur = db.Formateurs.Find(id);
            if (formateur != null && !formateur.Supp)
            {
                return View(formateur);
            }

            return HttpNotFound();
        }

        // POST: Admins/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int id)
        {
            var participant = db.Participants.Find(id);
            if (participant != null && !participant.Supp)
            {
                participant.Supp = true; 
                db.Entry(participant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Users");
            }

            var formateur = db.Formateurs.Find(id);
            if (formateur != null && !formateur.Supp)
            {
                formateur.Supp = true; 
                db.Entry(formateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Users");
            }

            return HttpNotFound();
        }

        // === Course CRUD  ===

        public ActionResult Formations()
        {
            var formations = db.Formations.Where(f => !f.Supp).ToList(); 
            var model = new FormationsPage
            {
                Formations = formations
            };
            return View(model);
        }

        public ActionResult DetailsFormation(int id)
        {
            var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && !f.Supp); 
            if (formation == null)
            {
                return HttpNotFound();
            }
            return View(formation);
        }

        // GET: Admin/CreateFormation
        public ActionResult CreateFormation()
        {
            ViewBag.Formateurs = new SelectList(db.Formateurs, "FormateurID", "Nom");
            return View();
        }

        // POST: Admin/CreateFormation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFormation(FormationsPage model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Formateurs = new SelectList(db.Formateurs, "FormateurID", "Nom");
                return View(model);
            }

            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                var fileName = model.Titre + fileExtension;
                var path = Path.Combine(Server.MapPath("~/assets/images/course"), fileName);
                model.ImageFile.SaveAs(path);
            }

            var formation = new Formation
            {
                Titre = model.Titre,
                Categorie = model.Categorie,
                Description = model.Description,
                FormateurID = model.FormateurID,
                Prix = model.Prix,
                Duration = model.Duration,
                Supp = false
            };
            db.Formations.Add(formation);
            db.SaveChanges();
            return RedirectToAction("Formations");
        }

        // GET: Admin/EditFormation/5
        public ActionResult EditFormation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && !f.Supp); 
            if (formation == null)
            {
                return HttpNotFound();
            }

            ViewBag.Formateurs = new SelectList(db.Formateurs, "FormateurID", "Nom", formation.FormateurID);
            var model = new FormationsPage
            {
                Titre = formation.Titre,
                Description = formation.Description,
                Categorie = formation.Categorie,
                Duration = formation.Duration,
                Prix = formation.Prix,
                FormateurID = formation.FormateurID
            };

            return View(model);
        }

        // POST: Admin/EditFormation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFormation(int id, FormationsPage model)
        {
            if (ModelState.IsValid)
            {
                var formation = db.Formations.Find(id);
                if (formation == null || formation.Supp)
                {
                    return HttpNotFound();
                }

                formation.Titre = model.Titre;
                formation.Description = model.Description;
                formation.Categorie = model.Categorie;
                formation.Duration = model.Duration;
                formation.Prix = model.Prix;
                formation.FormateurID = model.FormateurID;

                db.Entry(formation).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Formations");
            }

            ViewBag.Formateurs = new SelectList(db.Formateurs, "FormateurID", "Nom", model.FormateurID);
            return View(model);
        }

        // GET: Admin/DeleteFormation/5
        public ActionResult DeleteFormation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && !f.Supp); 
            if (formation == null)
            {
                return HttpNotFound();
            }
            return View(formation);
        }

        // POST: Admin/DeleteFormation/5
        [HttpPost, ActionName("DeleteFormation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFormationConfirmed(int id)
        {
            var formation = db.Formations.Find(id);
            if (formation == null)
            {
                return HttpNotFound();
            }

            formation.Supp = true;
            db.Entry(formation).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Formations");
        }

        // Optional: Restore a logically deleted course
        public ActionResult RestoreFormation(int id)
        {
            var formation = db.Formations.FirstOrDefault(f => f.FormationID == id && f.Supp); 
            if (formation == null)
            {
                return HttpNotFound();
            }

            formation.Supp = false;
            db.Entry(formation).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Formations");
        }

        // === Payment CRUD === 
        public ActionResult Payments()
        {
            var payments = db.Payments
                .Include(p => p.Formation) // Include related Formation data
                .Include(p => p.Participant) // Include related Participant data
                .Where(p => !p.Supp)
                .ToList();

            var model = new PayementPage
            {
                Payements = payments
            };

            return View(model);
        }

        public ActionResult DetailsPayment(int id)
        {
            var payment = db.Payments
                .Include(p => p.Formation)
                .Include(p => p.Formation.Formateur) 
                .Include(p => p.Participant) 
                .FirstOrDefault(p => p.Id == id && !p.Supp);

            if (payment == null)
            {
                return HttpNotFound();
            }
            // Debugging: Check if Formateur is loaded
            if (payment.Formation?.Formateur == null)
            {
                Debug.WriteLine("Formateur is null for Formation ID: " + payment.FormationID);
            }

            return View(payment);
        }
        public ActionResult ChangeStatus(int id)
        {
            var payment = db.Payments.Find(id);
            if (payment == null || payment.Supp)
            {
                return HttpNotFound();
            }

            // Debugging: Log FormationID and ParticipantID
            Debug.WriteLine($"Payment ID: {payment.Id}, FormationID: {payment.FormationID}, ParticipantID: {payment.ParticipantID}");

            switch (payment.PaymentStatus)
            {
                case "Pending Approval":
                    payment.PaymentStatus = "Accepting Approval";
                    break;
                case "Accepting Approval":
                    payment.PaymentStatus = "Accepted";
                    AddFormationParticipant(payment.ParticipantID, payment.FormationID); // Add to FormationParticipant table
                    break;
                case "Accepted":
                    payment.PaymentStatus = "Denying";
                    break;
                case "Denying":
                    payment.PaymentStatus = "Pending Approval";
                    break;
                default:
                    payment.PaymentStatus = "Pending Approval";
                    break;
            }

            db.Entry(payment).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Payments");
        }

        private void AddFormationParticipant(int participantId, int formationId)
        {
            // Debugging: Log the IDs being passed
            Debug.WriteLine($"Adding FormationParticipant - ParticipantID: {participantId}, FormationID: {formationId}");

            // Check if the record already exists to avoid duplicates
            var existingRecord = db.FormationParticipants
                .FirstOrDefault(fp => fp.ParticipantID == participantId && fp.FormationID == formationId);

            if (existingRecord == null)
            {
                var formationParticipant = new FormationParticipant
                {
                    ParticipantID = participantId,
                    FormationID = formationId,
                    Rating = 0,
                    Comment = ""
                };

                db.FormationParticipants.Add(formationParticipant);
                db.SaveChanges();
            }
        }

        //=== Actualites CRUD ===
        // GET: Admin/Actualites
        public ActionResult Actualites()
        {
            var actualites = db.Actualites.Where(a => !a.Supp).ToList();
            var model = new ActualitesPage
            {
                Actualites = actualites
            };
            return View(model);
        }

        // GET: Admin/Actualites/Details/5
        public ActionResult ActualiteDetails(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var actualite = db.Actualites.Include(a => a.Sections).FirstOrDefault(a => a.Id == id && !a.Supp);
            if (actualite == null)
                return HttpNotFound();

            return View(actualite);
        }

        // GET: Admin/Actualites/Create
        public ActionResult CreateActualite()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActualite(ActualitesPage model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                    if (fileExtension != ".jpg")
                    {
                        ModelState.AddModelError("ImageFile", "Only .jpg files are allowed.");
                        return View(model);
                    }

                    var fileName = model.Title.Replace(" ", "-").ToLower() + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/assets/images/Actualite"), fileName);

                    model.ImageFile.SaveAs(path);

                    model.ImagePath = "/assets/images/Actualite/" + fileName;
                }

                var actualite = new Actualites
                {
                    Title = model.Title,
                    Content = model.Content,
                    Date = DateTime.Now,
                    Views = 0,
                    Supp = false,
                    Sections = new List<Section>()  // No sections added here
                };

                db.Actualites.Add(actualite);
                db.SaveChanges();

                return RedirectToAction("Actualites");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the actualite.");
                return View(model);
            }
        }

        // GET: Admin/Actualites/Edit/5
        public ActionResult EditActualite(int? id)
        {
            System.Diagnostics.Debug.WriteLine("EditActualite GET action started.");

            if (id == null)
            {
                System.Diagnostics.Debug.WriteLine("ID parameter is missing.");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ID parameter is missing.");
            }

            var actualite = db.Actualites
                              .Include(a => a.Sections) 
                              .FirstOrDefault(a => a.Id == id);

            if (actualite == null || actualite.Supp)
            {
                System.Diagnostics.Debug.WriteLine("Actualite not found or soft-deleted.");
                return HttpNotFound("Actualite not found.");
            }

            var model = new ActualitesPage
            {
                Actualites = new List<Actualites> { actualite },
                Id = actualite.Id,
                Title = actualite.Title,
                Content = actualite.Content,
                Sections = actualite.Sections.Select(s => new Section
                {
                    Title = s.Title,
                    Content = s.Content
                }).ToList()
            };

            System.Diagnostics.Debug.WriteLine($"Fetched Actualite with ID: {actualite.Id}, Title: {actualite.Title}");

            return View(model);
        }


        // POST: Admin/Actualites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActualite(ActualitesPage model, HttpPostedFileBase ImageFile)
        {
            // Debugging: Log the start of the action
            System.Diagnostics.Debug.WriteLine("EditActualite POST action started.");

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState is invalid.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"Editing Actualite with ID: {model.Id}");

                // Fetch the existing Actualite from the database
                var actualite = db.Actualites
                                  .Include(a => a.Sections) // Include sections
                                  .FirstOrDefault(a => a.Id == model.Id);

                if (actualite == null || actualite.Supp)
                {
                    // Debugging: Log if the Actualite is not found
                    System.Diagnostics.Debug.WriteLine("Actualite not found or soft-deleted.");
                    return HttpNotFound("Actualite not found.");
                }

                // Debugging: Log the existing Actualite details
                System.Diagnostics.Debug.WriteLine($"Existing Actualite Title: {actualite.Title}");
                System.Diagnostics.Debug.WriteLine($"Existing Actualite Content: {actualite.Content}");

                // Update properties
                actualite.Title = model.Title;
                actualite.Content = model.Content;

                // Handle image upload
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // Debugging: Log image upload attempt
                    System.Diagnostics.Debug.WriteLine("Image upload started.");

                    var fileExtension = Path.GetExtension(ImageFile.FileName).ToLower();

                    if (fileExtension != ".jpg")
                    {
                        System.Diagnostics.Debug.WriteLine("Invalid file extension. Only .jpg files are allowed.");
                        ModelState.AddModelError("ImageFile", "Only .jpg files are allowed.");
                        return View(model);
                    }

                    var fileName = model.Title.Replace(" ", "-").ToLower() + fileExtension;
                    var path = Path.Combine(Server.MapPath("~/assets/images/Actualite"), fileName);

                    System.Diagnostics.Debug.WriteLine($"Saving image to: {path}");

                    ImageFile.SaveAs(path);
                    model.ImagePath = "/assets/images/Actualite/" + fileName;

                    System.Diagnostics.Debug.WriteLine("Image uploaded successfully.");
                }

                if (model.Sections != null && model.Sections.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"Updating {model.Sections.Count} sections.");

                    db.Sections.RemoveRange(actualite.Sections);

                    foreach (var section in model.Sections)
                    {
                        actualite.Sections.Add(new Section
                        {
                            Title = section.Title,
                            Content = section.Content,
                            Supp = false 
                        });

                        System.Diagnostics.Debug.WriteLine($"Added Section: {section.Title}");
                    }
                }

                db.Entry(actualite).State = EntityState.Modified;
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("Actualite and sections saved successfully.");

                return RedirectToAction("Actualites");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception occurred: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                ModelState.AddModelError("", "An error occurred while saving the actualite.");
                return View(model);
            }
        }

        // GET: Admin/Actualites/Delete/5
        public ActionResult DeleteActualite(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var actualite = db.Actualites.Find(id);
            if (actualite == null || actualite.Supp)
                return HttpNotFound();

            return View(actualite);
        }

        // POST: Admin/Actualites/Delete/5
        [HttpPost, ActionName("DeleteActualite")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActualiteConfirmed(int id)
        {
            var actualite = db.Actualites.Find(id);
            if (actualite != null)
            {
                actualite.Supp = true; // Soft delete
                db.SaveChanges();
            }

            return RedirectToAction("Actualites");
        }

        // ------------------------------------------
        // CRUD for Sections
        // ------------------------------------------

        // GET: Admin/Sections/Create
        public ActionResult CreateSection(int? actualiteId)
        {
            if (actualiteId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.ActualiteId = actualiteId;
            return View();
        }

        // POST: Admin/Sections/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSection(Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("ActualiteDetails", new { id = section.ActualiteId });
            }

            ViewBag.ActualiteId = section.ActualiteId;
            return View(section);
        }

        // GET: Admin/Sections/Edit/5
        public ActionResult EditSection(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var section = db.Sections.Find(id);
            if (section == null || section.Supp)
                return HttpNotFound();

            return View(section);
        }

        // POST: Admin/Sections/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSection(Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ActualiteDetails", new { id = section.ActualiteId });
            }

            return View(section);
        }

        // GET: Admin/Sections/Delete/5
        public ActionResult DeleteSection(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var section = db.Sections.Find(id);
            if (section == null || section.Supp)
                return HttpNotFound();

            return View(section);
        }

        // POST: Admin/Sections/Delete/5
        [HttpPost, ActionName("DeleteSection")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSectionConfirmed(int id)
        {
            var section = db.Sections.Find(id);
            if (section != null)
            {
                section.Supp = true; // Soft delete
                db.SaveChanges();
            }

            return RedirectToAction("ActualiteDetails", new { id = section.ActualiteId });
        }


        //=== Review Management ===
        [HttpGet]
        public ActionResult Reviews()
        {
            var reviews = db.FormationParticipants
                .Select(fp => new ReviewView
                {
                    FormationParticipantID = fp.FormationParticipantID,
                    Rating = fp.Rating,
                    Comment = fp.Comment,
                    FormationTitle = fp.Formation.Titre,
                    ParticipantName = fp.Participant.Nom + " " + fp.Participant.Prenom
                })
                .ToList();

            return View(reviews);
        }

        [HttpPost, ActionName("DeleteReview")]
        public ActionResult DeleteReview(int id)
        {
            try
            {
                // Debug: Log the ID received
                System.Diagnostics.Debug.WriteLine($"DeleteReview called with ID: {id}");

                // Retrieve the review by ID
                var review = db.FormationParticipants.Find(id);

                if (review != null)
                {
                    // Debug: Log review details before modification
                    System.Diagnostics.Debug.WriteLine($"Review found: ID = {review.FormationParticipantID}, Rating = {review.Rating}, Comment = {review.Comment}");

                    // Clear the review's comment and rating
                    review.Comment = string.Empty;
                    review.Rating = 0;

                    // Save changes to the database
                    db.SaveChanges();

                    // Debug: Log successful save
                    System.Diagnostics.Debug.WriteLine($"Review cleared successfully for ID: {id}");

                    TempData["Message"] = "Review cleared successfully.";
                }
                else
                {
                    // Debug: Log review not found
                    System.Diagnostics.Debug.WriteLine($"Review not found for ID: {id}");

                    TempData["Error"] = "Review not found.";
                }
            }
            catch (Exception ex)
            {
                // Debug: Log any exceptions that occur
                System.Diagnostics.Debug.WriteLine($"An error occurred in DeleteReview: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                TempData["Error"] = "An unexpected error occurred. Please try again later.";
            }

            // Redirect back to the Reviews action
            return RedirectToAction("Reviews");
        }

        //=== Certificate Management ===
        public ActionResult Certificates()
        {
            // Get certified courses (Completed = true)
            var certifiedCourses = db.Formations
                .Where(f => f.Completed && !f.Supp)
                .Include(f => f.Formateur)
                .ToList();

            // Get non-certified courses (Completed = false)
            var nonCertifiedCourses = db.Formations
                .Where(f => !f.Completed && !f.Supp)
                .Include(f => f.Formateur)
                .ToList();

            // Create the view model
            var viewModel = new CertificateView
            {
                CertifiedCourses = certifiedCourses,
                NonCertifiedCourses = nonCertifiedCourses
            };

            return View(viewModel);
        }

        // POST: Formations/CertifyCourse
        [HttpPost]
        public ActionResult CertifyCourse(int id)
        {
            var course = db.Formations.Find(id);
            if (course != null)
            {
                course.Completed = true; 
                db.SaveChanges();
                TempData["Message"] = "Course certified successfully!";
            }
            else
            {
                TempData["Error"] = "Course not found.";
            }

            return RedirectToAction("Certificates");
        }






    }
}

