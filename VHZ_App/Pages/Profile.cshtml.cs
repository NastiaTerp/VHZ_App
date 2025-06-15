using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly VhzContext _context;

        public ProfileModel(VhzContext context)
        {
            _context = context;
        }

        public User User { get; set; } = null!;

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.IdUser == userId);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }

            User = user;
            return Page();
        }
    }
}
