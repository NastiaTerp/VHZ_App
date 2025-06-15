using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class AccountModel : PageModel
    {
        private readonly VhzContext _context;

        public AccountModel(VhzContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            string login = Request.Form["Login"];
            string password = Request.Form["Password"];

            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return Page();
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", user.IdUser);
            return RedirectToPage("/Profile");
        }
    }
}
