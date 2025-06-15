using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly VhzContext _context;

        public ForgotPasswordModel(VhzContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        public string? Message { get; set; }

        public void OnPost()
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);

            if (user == null)
            {
                Message = "Пользователь с таким email не найден.";
                return;
            }

            // Здесь может быть отправка кода на почту, или временный вывод пароля (если нет хеширования).
            Message = $"Ваш логин: {user.Login}. Для сброса пароля обратитесь к администратору.";
        }
    }
}
