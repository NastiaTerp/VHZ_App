using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly VhzContext _context;
        private readonly IConfiguration _config;

        public ForgotPasswordModel(VhzContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                Message = "Пользователь с таким email не найден.";
                return Page();
            }

            var token = Guid.NewGuid().ToString();

            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                UserId = user.IdUser,
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            });
            await _context.SaveChangesAsync();

            var resetLink = Url.Page("/ResetPassword", null, new { token }, Request.Scheme);

            var smtpHost = _config["Smtp:Host"];
            var smtpPort = int.Parse(_config["Smtp:Port"]);
            var smtpUser = _config["Smtp:User"];
            var smtpPass = _config["Smtp:Pass"];

            var message = new MailMessage();
            message.From = new MailAddress(smtpUser);
            message.To.Add(Email);
            message.Subject = "Сброс пароля";
            message.Body = $"Для сброса пароля перейдите по ссылке: {resetLink}";

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };
            await client.SendMailAsync(message);

            Message = $"Письмо отправлено на {Email}.";
            return Page();
        }
    }
}
