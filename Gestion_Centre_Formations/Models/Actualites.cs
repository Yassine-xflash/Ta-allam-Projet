using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class Actualites
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }  
        public bool Supp { get; set; }
    }
}