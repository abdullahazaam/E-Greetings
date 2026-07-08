using System.Net;
using System.Net.Mail;

namespace E_Greetings.Services
{
    public interface IEmailService
    {
        Task<bool> SendCardEmailAsync(string toEmail, string subject, string message, string cardImageUrl, string recipientName, string senderName);
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink);
        Task<bool> SendEmailConfirmationAsync(string toEmail, string confirmationLink);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ========== SEND CARD EMAIL ==========
        public async Task<bool> SendCardEmailAsync(string toEmail, string subject, string message, string cardImageUrl, string recipientName, string senderName)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var username = _configuration["EmailSettings:SmtpUsername"];
                var password = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                // SIMPLE HTML BODY
                string body = "<html>";
                body += "<head><style>";
                body += "body { font-family: Arial, sans-serif; background: #f5f5f5; padding: 20px; }";
                body += ".container { max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 20px; padding: 40px; box-shadow: 0 10px 40px rgba(0,0,0,0.1); }";
                body += ".header { text-align: center; color: #6C63FF; font-size: 28px; font-weight: 700; }";
                body += ".message-box { color: #333; font-size: 16px; line-height: 1.8; margin: 20px 0; padding: 20px; background: #f8f9ff; border-radius: 10px; border-left: 4px solid #6C63FF; }";
                body += ".card-image { width: 100%; border-radius: 15px; margin: 15px 0; }";
                body += ".footer { text-align: center; color: #888; font-size: 12px; margin-top: 20px; border-top: 1px solid #eee; padding-top: 20px; }";
                body += ".highlight { color: #6C63FF; font-weight: 600; }";
                body += "</style></head>";

                body += "<body>";
                body += "<div class='container'>";
                body += "<div class='header'>💌 E-Greetings</div>";
                body += "<h3>Dear <strong>" + recipientName + "</strong>,</h3>";
                body += "<div class='message-box'>" + message + "</div>";
                body += "<img src='" + cardImageUrl + "' alt='Card' class='card-image' />";
                body += "<p style='text-align:center; color:#666;'>💕 Sent with love from <strong>" + senderName + "</strong></p>";
                body += "<div class='footer'>© 2026 <span class='highlight'>E-Greetings</span> — Spread joy, one card at a time</div>";
                body += "</div>";
                body += "</body></html>";

                mailMessage.Body = body;
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ========== SEND PASSWORD RESET EMAIL ==========
        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var username = _configuration["EmailSettings:SmtpUsername"];
                var password = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = "Password Reset Request - E-Greetings",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                string body = "<html>";
                body += "<body style='font-family: Arial, sans-serif; background: #f5f5f5; padding: 20px;'>";
                body += "<div style='max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 20px; padding: 40px;'>";
                body += "<h2 style='color: #6C63FF; text-align: center;'>🔐 Reset Your Password</h2>";
                body += "<p style='color: #333; font-size: 16px;'>We received a request to reset your password. Click the button below to create a new password.</p>";
                body += "<div style='text-align: center; margin: 30px 0;'>";
                body += "<a href='" + resetLink + "' style='background: #6C63FF; color: #FFFFFF; padding: 14px 40px; text-decoration: none; border-radius: 50px; display: inline-block;'>Reset Password</a>";
                body += "</div>";
                body += "<p style='color: #888; font-size: 14px; text-align: center;'>If you didn't request this, please ignore this email.</p>";
                body += "<p style='color: #888; font-size: 12px; text-align: center; margin-top: 20px; border-top: 1px solid #eee; padding-top: 20px;'>© 2026 E-Greetings</p>";
                body += "</div></body></html>";

                mailMessage.Body = body;
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ========== SEND EMAIL CONFIRMATION ==========
        public async Task<bool> SendEmailConfirmationAsync(string toEmail, string confirmationLink)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var username = _configuration["EmailSettings:SmtpUsername"];
                var password = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = "Confirm Your Email - E-Greetings",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                string body = "<html>";
                body += "<body style='font-family: Arial, sans-serif; background: #f5f5f5; padding: 20px;'>";
                body += "<div style='max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 20px; padding: 40px;'>";
                body += "<h2 style='color: #6C63FF; text-align: center;'>🎉 Welcome to E-Greetings!</h2>";
                body += "<p style='color: #333; font-size: 16px;'>Thank you for registering! Please confirm your email address by clicking the button below.</p>";
                body += "<div style='text-align: center; margin: 30px 0;'>";
                body += "<a href='" + confirmationLink + "' style='background: #6C63FF; color: #FFFFFF; padding: 14px 40px; text-decoration: none; border-radius: 50px; display: inline-block;'>Confirm Email</a>";
                body += "</div>";
                body += "<p style='color: #888; font-size: 14px; text-align: center;'>If you didn't create an account, please ignore this email.</p>";
                body += "<p style='color: #888; font-size: 12px; text-align: center; margin-top: 20px; border-top: 1px solid #eee; padding-top: 20px;'>© 2026 E-Greetings</p>";
                body += "</div></body></html>";

                mailMessage.Body = body;
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}