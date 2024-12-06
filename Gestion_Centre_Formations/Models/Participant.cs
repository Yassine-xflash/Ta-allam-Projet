using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Participant : User
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParticipantID { get; set; }
        [Required]
        public DateTime DateInscription { get; set; }
        public int NbrFormations { get; set; }

        //navigation pour le tableau de jointure FormationParticipants
        public ICollection<FormationParticipant> FormationParticipants { get; set; }
    }
}