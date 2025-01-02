using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class FormationsPage
    {

        public IEnumerable<Formation> Formations { get; set; }

        // Properties for Create/Edit Formation
        [Required]
        public string Titre { get; set; }

        [Required]
        public string Categorie { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int FormateurID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Prix { get; set; }

        [Required]
        public int Duration { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}