using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Data
{
    public class Gestion_Centre_FormationsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Gestion_Centre_FormationsContext() : base("name=Gestion_Centre_FormationsContext")
        {
        }

        public System.Data.Entity.DbSet<Gestion_Centre_Formations.Models.Participant> Participants { get; set; }

        public System.Data.Entity.DbSet<Gestion_Centre_Formations.Models.Formateur> Formateurs { get; set; }

        public System.Data.Entity.DbSet<Gestion_Centre_Formations.Models.Admin> Admins { get; set; }

        public System.Data.Entity.DbSet<Gestion_Centre_Formations.Models.Formation> Formations { get; set; }

        public System.Data.Entity.DbSet<Gestion_Centre_Formations.Models.FormationParticipant> FormationParticipants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure Formation to Formateur relationship
            modelBuilder.Entity<Formation>()
                .HasRequired(f => f.Formateur)
                .WithMany(f => f.Formations)
                .HasForeignKey(f => f.FormateurID);

            // Configure FormationParticipant relationships
            modelBuilder.Entity<FormationParticipant>()
                .HasRequired(fp => fp.Formation)
                .WithMany(f => f.FormationParticipants)
                .HasForeignKey(fp => fp.FormationID);

            modelBuilder.Entity<FormationParticipant>()
                .HasRequired(fp => fp.Participant)
                .WithMany(p => p.FormationParticipants)
                .HasForeignKey(fp => fp.ParticipantID);

            base.OnModelCreating(modelBuilder);
        }

        public void SeedDatabase()
        {
            try
            {
                // Clear existing data to prevent duplicates
                this.FormationParticipants.RemoveRange(this.FormationParticipants);
                this.Formations.RemoveRange(this.Formations);
                this.Admins.RemoveRange(this.Admins);
                this.Participants.RemoveRange(this.Participants);
                this.Formateurs.RemoveRange(this.Formateurs);

                // Create dummy formateur
                var testFormateur = new Formateur
                {
                    Nom = "Brown",
                    Prenom = "Mike",
                    Email = "formateur@example.com",
                    NumTel = "1234567890",
                    Password = "FormateurPass123",
                    Specialisation = "IT",
                    NbrFormations = 1
                };
                this.Formateurs.Add(testFormateur);

                // Create dummy participant
                var testParticipant = new Participant
                {
                    Nom = "Smith",
                    Prenom = "Jane",
                    Email = "participant@example.com",
                    NumTel = "0987654321",
                    Password = "ParticipantPass123",
                    DateInscription = DateTime.Now,
                    NbrFormations = 1
                };
                this.Participants.Add(testParticipant);

                // Try to save changes first to validate entities
                this.SaveChanges();

                // Create dummy formation
                var testFormation = new Formation
                {
                    Titre = "Introduction to Programming",
                    Description = "A beginner-friendly course on programming fundamentals",
                    Categorie = "Informatique",
                    Duration = 30, // hours
                    Prix = 199.99,
                    Formateur = testFormateur // Set the relationship
                };
                this.Formations.Add(testFormation);

                // Create dummy formation participant (junction table)
                var testFormationParticipant = new FormationParticipant
                {
                    Formation = testFormation,
                    Participant = testParticipant
                };
                this.FormationParticipants.Add(testFormationParticipant);

                // Save changes
                this.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Detailed error logging
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                // Log or throw with full details
                throw new System.Data.Entity.Validation.DbEntityValidationException(
                    "Validation Errors: " + fullErrorMessage, ex
                );
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                throw new Exception("Error in SeedDatabase: " + ex.Message, ex);
            }
        }
    }
}
