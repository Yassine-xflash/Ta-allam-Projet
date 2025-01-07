using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Centre_Formations.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } // Title of the section

        [Required]
        public string Content { get; set; } // Content of the section

        public bool Supp { get; set; } // Soft delete

        // Foreign key for Actualites
        [ForeignKey("Actualite")]
        public int ActualiteId { get; set; }

        public Actualites Actualite { get; set; }
    }
}
