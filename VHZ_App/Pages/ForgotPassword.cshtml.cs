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
                Message = "������������ � ����� email �� ������.";
                return;
            }

            // ����� ����� ���� �������� ���� �� �����, ��� ��������� ����� ������ (���� ��� �����������).
            Message = $"��� �����: {user.Login}. ��� ������ ������ ���������� � ��������������.";
        }
    }
}
