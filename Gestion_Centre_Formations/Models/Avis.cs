using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Avis
    {
        public int Id { get; set; }

        [Required]
        public int ParticipantID { get; set; }

        [Required]
        public int FormationID { get; set; } 

        [Required]
        public string Review { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public bool Supp { get; set; }

        [ForeignKey("ParticipantID")]
        public virtual Participant Participant { get; set; }

        [ForeignKey("FormationID")]
        public virtual Formation Formation { get; set; }

    }

}