using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;

namespace Gestion_Centre_Formations.Controllers
{
    public class AdminsController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Admins
        public ActionResult Index()
        {
            var FormationsCount = db.Formations.Count();
            var participantsCount = db.Participants.Count();
            var FormateursCiunt = db.Formateurs.Count();
            var participants = db.Participants.ToList();
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
                Formations = Formations
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
                        Password = model.Password,
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
                        Password = model.Password,
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
                    Password = participant.Password,
                    ConfirmPassword = participant.Password,
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
                    Password = formateur.Password,
                    ConfirmPassword = formateur.Password,
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
                    participant.Password = model.Password;

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
                    formateur.Password = model.Password;
                    formateur.Specialisation = model.Specialisation;

                    db.Entry(formateur).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Users");
            }

            return View(model);
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



    }
}
