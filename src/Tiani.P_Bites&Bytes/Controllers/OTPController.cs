using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Tiani.P_Bites_Bytes.Models;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class OTPController : Controller
    {
        private BitsAndBytesDbContext context = new BitsAndBytesDbContext();

        // GET: OTP/GenerateOTP
        [HttpGet]
        public ActionResult GenerateOTP(string email)
        {
            // Generate OTP
            string otp = GenerateOTP();

            // Send OTP to customer's email
            SendOTPByEmail(email, otp);

            // Store OTP in session (you can use other storage mechanisms based on your requirement)
            Session["OTP"] = otp;
            Session["CustomerEmail"] = email;

            // Redirect to OTP verification page
            return RedirectToAction("VerifyOTP");
        }

        // GET: OTP/VerifyOTP
        [HttpGet]
        public ActionResult VerifyOTP()
        {
            // Ensure OTP is generated and stored in session
            if (Session["OTP"] != null && Session["CustomerEmail"] != null)
            {
                return View();
            }
            else
            {
                // If OTP is not generated, redirect to generate OTP page
                return RedirectToAction("GenerateOTP");
            }
        }

        // POST: OTP/VerifyOTP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyOTP(string otp)
        {
            // Retrieve stored OTP from session
            string storedOTP = Session["OTP"] as string;

            // Check if provided OTP matches the stored OTP
            if (otp == storedOTP)
            {
                // OTP verification successful, redirect to products page
                return RedirectToAction("Products", "Shop");
            }
            else
            {
                // OTP verification failed, display error message
                ViewBag.Error = "Invalid OTP. Please try again.";
                return View();
            }
        }

        // Method to generate a random OTP
        private string GenerateOTP()
        {
            // Generate a random 6-digit OTP
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Method to send OTP to customer's email
        private void SendOTPByEmail(string email, string otp)
        {
            // Configure SMTP client
            SmtpClient client = new SmtpClient("smtp.example.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@example.com", "your-password"),
                EnableSsl = true,
            };

            // Create mail message
            MailMessage message = new MailMessage("your-email@example.com", email)
            {
                Subject = "One Time Password (OTP) Verification",
                Body = "Your OTP is: " + otp,
            };

            // Send mail
            client.Send(message);
        }
    }
}
