using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class FormateurFormationDetailsViewModel
    {
        public Formation Formation { get; set; }
        public double TotalPrice { get; set; }
        public List<Participant> Participants { get; set; } 
    }
}