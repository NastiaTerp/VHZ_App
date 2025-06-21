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
                Message = "������������ � ����� email �� ������.";
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

            var smtpSettings = _config.GetSection("Smtp").Get<SmtpSettings>();

            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(smtpSettings.User, "������������ ���������� �����");
                message.To.Add(Email);
                message.Subject = "�������������� ������ ��� ���";

                // �������� HTML-������ � ���������� �����
                message.IsBodyHtml = true;
                message.Body = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: 'Arial', sans-serif;
                                line-height: 1.6;
                                color: #333;
                                max-width: 600px;
                                margin: 0 auto;
                                padding: 20px;
                            }}
                            .header {{
                                background-color: #6a0dad;
                                color: white;
                                padding: 15px;
                                text-align: center;
                                border-radius: 5px 5px 0 0;
                            }}
                            .content {{
                                border: 1px solid #ddd;
                                border-top: none;
                                padding: 20px;
                                background-color: #f9f3ff;
                                border-radius: 0 0 5px 5px;
                            }}
                            .reset-button {{
                                display: inline-block;
                                background-color: #6a0dad;
                                color: white !important;
                                text-decoration: none;
                                padding: 12px 25px;
                                margin: 15px 0;
                                border-radius: 4px;
                                font-weight: bold;
                            }}
                            .info-text {{
                                margin: 15px 0;
                                line-height: 1.5;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #888;
                                text-align: center;
                                border-top: 1px solid #eee;
                                padding-top: 10px;
                            }}
                            .highlight {{
                                color: #6a0dad;
                                font-weight: bold;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='header'>
                            <h2>�������������� ������</h2>
                        </div>
                        
                        <div class='content'>
                            <p class='info-text'>������������,</p>
                            
                            <p class='info-text'>�� �������� ������ �� ����� ������ ��� ����� ������� ������ �� ����� <span class='highlight'>������������� ����������� ������</span>.</p>
                            
                            <p class='info-text'>��� ��������� ������ ������ ������� �� ������ ����:</p>
                            
                            <div style='text-align: center;'>
                                <a href='{resetLink}' class='reset-button'>�������� ������</a>
                            </div>
                            
                            <p class='info-text'>������ ����� ������������� � ������� <span class='highlight'>1 ����</span>.</p>
                            
                            <p class='info-text'>���� �� �� ����������� ����� ������, ������ �������������� ��� ������.</p>
                        </div>
                        
                        <div class='footer'>
                            <p>��� ������ ���� ���������� �������������. ����������, �� ��������� �� ����.</p>
                            <p>� {DateTime.Now.Year} ������������ ���������� �����</p>
                        </div>
                    </body>
                    </html>
                ";

                using var client = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(smtpSettings.User, smtpSettings.Pass),
                    EnableSsl = true
                };

                await client.SendMailAsync(message);

                Message = $"�� ����� {Email} ���� ���������� ������ � ������������ �� �������������� ������.";
            }
            catch (Exception ex)
            {
                Message = "��������� ������ ��� �������� ������. ����������, ���������� �����.";
                // ����������� ������
                Console.WriteLine($"������ �������� ������: {ex.Message}");
            }

            return Page();
        }
    }
}