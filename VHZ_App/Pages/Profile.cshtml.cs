using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
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
        public BankCard? BankCard { get; set; }
        public List<Order> Orders { get; set; } = new();

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var user = _context.Users.FirstOrDefault(u => u.IdUser == userId);
            if (user == null) return RedirectToPage("/Login");

            User = user;

            BankCard = _context.BankCards.FirstOrDefault(c => c.IdUser == userId);
            Orders = _context.Orders
                .Where(o => o.IdUser == userId)
                .OrderByDescending(o => o.IdOrder)
                .Take(5)
                .ToList();

            return Page();
        }
        public IActionResult OnPostDeleteCard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var card = _context.BankCards.FirstOrDefault(c => c.IdUser == userId);
            if (card != null)
            {
                _context.BankCards.Remove(card);
                _context.SaveChanges();
            }

            return RedirectToPage("/Profile");
        }
        public string GetAvatarPath()
        {
            var avatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars", $"{User.IdUser}.jpg");
            if (System.IO.File.Exists(avatarPath))
                return $"/uploads/avatars/{User.IdUser}.jpg";
            else
                return "/Image/user-placeholder.png";
        }
    }
}
