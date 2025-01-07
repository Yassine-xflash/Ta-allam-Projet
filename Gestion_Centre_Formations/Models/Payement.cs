using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Centre_Formations.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        [ForeignKey("Formation")]
        public int FormationID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("Participant")]
        public int ParticipantID { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [StringLength(20, ErrorMessage = "Payment status cannot exceed 20 characters.")]
        public string PaymentStatus { get; set; }

        public bool Supp { get; set; }

        // Navigation properties
        public virtual Formation Formation { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
