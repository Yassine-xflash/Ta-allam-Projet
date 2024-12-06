using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string NumTel { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserType { get; set; }

        // Optional for Formateur
        [StringLength(25, ErrorMessage = "Specialisation cannot exceed 25 characters")]
        public string Specialisation { get; set; }

    }
}