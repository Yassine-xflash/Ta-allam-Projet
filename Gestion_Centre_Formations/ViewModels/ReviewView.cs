using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class ReviewView
    {
        public int FormationParticipantID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string FormationTitle { get; set; }
        public string ParticipantName { get; set; }
    }
}