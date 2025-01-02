using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class AdminPage
    {
        // Properties for statistics
        public double FormationsCount { get; set; }
        public int ParticipantsCount { get; set; }
        public double FormateursCount { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
        public IEnumerable<Formation> Formations { get; set; }
        public IEnumerable<Formateur> Formateurs { get; set; }

        // Properties for the form
        [Required]
        public string Nom { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string NumTel { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserType { get; set; } // "Participant" or "Formateur"

        // Participant-specific properties
        public DateTime DateInscription { get; set; }
        public int NbrFormations { get; set; }

        // Formateur-specific properties
        public string Specialisation { get; set; }
    }
}
