using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Formation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormationID { get; set; }
        [Required]
        [StringLength(100)]
        public string Titre { get; set; }
        [Required]
        [StringLength(200)]

        public string Description { get; set; }
        [Required]
        [StringLength(25)]

        public string Categorie { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double Prix { get; set; }

        public bool Supp { get; set; }
        [Required]
        [ForeignKey("Formateur")]
        public int FormateurID { get; set; }
        //navigation pour le formateur
        public Formateur Formateur { get; set; }
        //navigation pour le tableau de jointure FormationParticipants
        public ICollection<FormationParticipant> FormationParticipants { get; set; }
    }
}