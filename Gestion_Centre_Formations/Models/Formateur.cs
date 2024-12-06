using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Formateur : User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormateurID { get; set; }
        [Required]
        [StringLength(25)]
        public string Specialisation { get; set; }
        public int NbrFormations { get; set; }
        public ICollection<Formation> Formations { get; set; }
    }
}