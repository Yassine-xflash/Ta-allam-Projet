using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class AboutPage
    {
        public double FormationsCount { get; set; }
        public int ParticipantsCount { get; set; }
        public double AverageRating { get; set; }
        public List<Formation> PopularFormations { get; set; }
    }
}