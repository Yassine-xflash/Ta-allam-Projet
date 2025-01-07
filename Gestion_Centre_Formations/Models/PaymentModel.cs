using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Gestion_Centre_Formations.Models
{
    public class PaymentModel
    {
        [Required(ErrorMessage = "Card number is required")]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Cardholder name is required")]
        [StringLength(50, ErrorMessage = "Cardholder name cannot exceed 50 characters")]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Invalid expiry date format (MM/YY)")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        public string CVV { get; set; }

        [EmailAddress(ErrorMessage = "Invalid PayPal email format")]
        public string PayPalEmail { get; set; }

        // Custom validation for ExpiryDate to ensure it's not in the past
        public bool IsValidExpiryDate()
        {
            if (DateTime.TryParseExact(
                ExpiryDate,
                "MM/yy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime expiryDate))
            {
                return expiryDate > DateTime.Now;
            }
            return false;
        }
    }
}
