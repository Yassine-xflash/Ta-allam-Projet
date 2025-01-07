using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class AboutPage
    {
        public int FormationsCount { get; set; }

        public int ParticipantsCount { get; set; }

        public int FormateursCount { get; set; }

        public double AverageRating { get; set; }

        public List<Formation> PopularFormations { get; set; }

        public List<ReviewViewModel> RecentReviews { get; set; }
    }

    public class ReviewViewModel
    {
        public string ParticipantName { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }
    }
}