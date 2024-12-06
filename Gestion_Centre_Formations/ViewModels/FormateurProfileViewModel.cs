using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class FormateurProfileViewModel
    {
        public Formateur Formateur { get; set; }
        public List<Formation> Formations { get; set; }
    }
}