using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Certificate
    {
        public int CertificateID { get; set; }
        public string Title { get; set; }
        public DateTime DateIssued { get; set; }
        public bool Supp { get; set; }

        // Foreign Keys
        public int ParticipantID { get; set; }
        public virtual Participant Participant { get; set; }
        public int FormationID { get; set; }
        public virtual Formation Formation { get; set; }
    }
}