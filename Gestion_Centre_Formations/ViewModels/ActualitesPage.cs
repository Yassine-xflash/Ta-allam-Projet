using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class ActualitesPage
    {
        public IEnumerable<Actualites> Actualites { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le contenu est requis.")]
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int Views { get; set; }

        public bool Supp { get; set; }

        public string ImagePath { get; set; } // Path to the uploaded image

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageFile { get; set; } // File upload property

        // Navigation property for Sections
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}