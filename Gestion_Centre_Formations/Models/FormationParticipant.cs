using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class FormationParticipant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormationParticipantID { get; set; }

        [Required]
        [ForeignKey("Formation")]
        public int FormationID { get; set; }
        public Formation Formation { get; set; }

        [Required]
        [ForeignKey("Participant")]
        public int ParticipantID { get; set; }
        //navigation pour le participant
        public Participant Participant { get; set; }
        public bool HasCertificate { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}