using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gestion_Centre_Formations.Models
{
    public class PaymentModel
    {
        [Required, CreditCard]
       public string CardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string CVV { get; set; }
    }

}