using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class User
    {
        [Required]
        [MaxLength(50)]
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
    }
}