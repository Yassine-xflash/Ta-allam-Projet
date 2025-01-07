using Gestion_Centre_Formations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class PayementPage
    {
        public IEnumerable<Payment> Payements { get; set; }
    }
}