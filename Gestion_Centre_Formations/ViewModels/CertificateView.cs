using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class CertificateView
    {
        public List<Formation> CertifiedCourses { get; set; }
        public List<Formation> NonCertifiedCourses { get; set; }
        public int FormationID { get; set; }
        public string CourseTitle { get; set; }
        public Certificate Certificate { get; set; }
        public string CertificateHash { get; set; }
    }
}