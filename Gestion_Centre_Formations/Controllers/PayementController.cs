using Gestion_Centre_Formations.Data;
using Gestion_Centre_Formations.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Gestion_Centre_Formations.Controllers
{
    public class PayementController : Controller
    {
        private Gestion_Centre_FormationsContext db = new Gestion_Centre_FormationsContext();

        // GET: Payement
        public ActionResult Index(int courseId, int userId)
        {
            // Get course details for pricing
            var course = db.Formations.Find(courseId);
            if (course == null)
            {
                return RedirectToAction("Error", "Home"); // Redirect if course not found
            }

            // Pass course details and userId to the view
            ViewBag.CourseId = courseId;
            ViewBag.UserId = userId;
            ViewBag.CoursePrice = course.Prix; // Use the `Prix` property
            ViewBag.CourseTitle = course.Titre;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPayment(PaymentModel model, int courseId, int userId)
        {
            try
            {
                // Log form submission start
                System.Diagnostics.Debug.WriteLine("Form submission started.");
                System.Diagnostics.Debug.WriteLine($"courseId: {courseId}, userId: {userId}");

                // Determine payment method
                bool isPayPalSelected = !string.IsNullOrEmpty(model.PayPalEmail);
                bool isCreditCardSelected = !string.IsNullOrEmpty(model.CardNumber);

                // If PayPal is selected, remove card fields from ModelState validation
                if (isPayPalSelected)
                {
                    ModelState.Remove("CardHolder");
                    ModelState.Remove("CardNumber");
                    ModelState.Remove("ExpiryDate");
                    ModelState.Remove("CVV");
                }

                if (ModelState.IsValid)
                {
                    System.Diagnostics.Debug.WriteLine("ModelState is valid.");

                    // Get course details for price based on courseId
                    var course = db.Formations.Find(courseId);
                    if (course == null)
                    {
                        // Log course not found error
                        System.Diagnostics.Debug.WriteLine($"Error: Course with ID {courseId} not found.");
                        ModelState.AddModelError("", "Course not found.");
                        return RedirectToAction("Index", new { courseId, userId });
                    }

                    // Log course details
                    System.Diagnostics.Debug.WriteLine($"Course found: {course.Titre}, Price: {course.Prix}");

                    // Determine payment method
                    string paymentMethod;
                    if (isCreditCardSelected)
                    {
                        paymentMethod = GetCardType(model.CardNumber); // Determine card type
                        System.Diagnostics.Debug.WriteLine($"Payment method determined: {paymentMethod}.");
                    }
                    else if (isPayPalSelected)
                    {
                        paymentMethod = "PayPal";
                        System.Diagnostics.Debug.WriteLine("Payment method determined: PayPal.");

                        // Reset card fields to default values if PayPal is selected
                        model.CardHolder = null;
                        model.CardNumber = null;
                        model.ExpiryDate = null;
                        model.CVV = null;
                    }
                    else
                    {
                        paymentMethod = "Other";
                        System.Diagnostics.Debug.WriteLine("Payment method determined: Other.");
                    }

                    // Create and save the payment record
                    var payment = new Payment
                    {
                        FormationID = courseId,
                        ParticipantID = userId,
                        PaymentDate = DateTime.Now,
                        Amount = course.Prix,
                        PaymentMethod = paymentMethod,
                        PaymentStatus = "Pending Approval",
                        Supp = false
                    };

                    db.Payments.Add(payment);
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Payment record saved successfully.");

                    // Redirect to success page
                    return RedirectToAction("Success");
                }
                else
                {
                    // Log invalid model state errors
                    System.Diagnostics.Debug.WriteLine("ModelState is invalid. Errors:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        System.Diagnostics.Debug.WriteLine($"- {error.ErrorMessage}");
                    }
                }

                // Reload course details in case of invalid model state
                var reloadCourse = db.Formations.Find(courseId);
                if (reloadCourse == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Error: Reload failed, course with ID {courseId} not found.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Reload successful: Course {reloadCourse.Titre} found.");
                }

                ViewBag.CourseId = courseId;
                ViewBag.UserId = userId;
                ViewBag.CoursePrice = reloadCourse?.Prix ?? 0;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine($"Exception occurred: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Handle the exception gracefully
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View("Index", model);
            }
        }

        private string GetCardType(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return "Unknown";

            // Remove all non-digit characters
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            // Check card type based on prefixes
            if (cardNumber.StartsWith("4"))
            {
                return "Visa";
            }
            else if (cardNumber.StartsWith("5") && cardNumber.Length >= 2 && "12345".Contains(cardNumber[1]))
            {
                return "MasterCard";
            }
            else if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
            {
                return "American Express";
            }
            else if (cardNumber.StartsWith("6") || cardNumber.StartsWith("5"))
            {
                return "Discover";
            }
            else if (cardNumber.StartsWith("30") || cardNumber.StartsWith("36") || cardNumber.StartsWith("38"))
            {
                return "Diners Club";
            }
            else if (cardNumber.StartsWith("35"))
            {
                return "JCB";
            }
            else
            {
                return "Unknown";
            }
        }
        // GET: Payement/Success
        public ActionResult Success()
        {
            return View();
        }

        // GET: Payement/Error
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}
