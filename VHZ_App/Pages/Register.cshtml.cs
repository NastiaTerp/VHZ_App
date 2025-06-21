using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly VhzContext _context;

        public RegisterModel(VhzContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            var form = Request.Form;

            string surname = form["Surname"];
            string name = form["Name"];
            string pathronimic = form["Pathronimic"];
            string nameCompany = form["CompanyFull"];
            string inn = form["INN"];
            string kpp = form["KPP"];
            string phone = form["Phone"];
            string email = form["Email"];
            string login = form["Login"];
            string password = form["Password"];
            string confirmPassword = form["ConfirmPassword"];
            string post = form["Position"];

            // ��������� ������������ ����
            if (string.IsNullOrWhiteSpace(surname))
            {
                ModelState.AddModelError(string.Empty, "������� ����������� ��� ����������");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError(string.Empty, "��� ����������� ��� ����������");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(inn))
            {
                ModelState.AddModelError(string.Empty, "��� ���������� ��� ����������");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                ModelState.AddModelError(string.Empty, "������� ���������� ��� ����������");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Email ���������� ��� ����������");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(login) || login.Length < 4)
            {
                ModelState.AddModelError(string.Empty, "����� ������ ��������� ������� 4 �������");
                return Page();
            }

            if (password != confirmPassword || password.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "������ �� ��������� ��� ������� �������� (������� 6 ��������)");
                return Page();
            }

            // �������� ������� email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ModelState.AddModelError(string.Empty, "������������ email");
                return Page();
            }

            // �������� ������� �������� (����� ���������� ��� ������ ������)
            if (!Regex.IsMatch(phone, @"^\+?[0-9\s\-\(\)]{7,20}$"))
            {
                ModelState.AddModelError(string.Empty, "������������ �������");
                return Page();
            }

            // �������� ������������ ������
            if (_context.Users.Any(u => u.Login == login))
            {
                ModelState.AddModelError(string.Empty, "������������ � ����� ������� ��� ����������");
                return Page();
            }

            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                Surname = surname.Trim(),
                Name = name.Trim(),
                Pathronimic = string.IsNullOrWhiteSpace(pathronimic) ? null : pathronimic.Trim(),
                NameCompany = string.IsNullOrWhiteSpace(nameCompany) ? null : nameCompany.Trim(),
                Inn = inn.Trim(),
                Kpp = string.IsNullOrWhiteSpace(kpp) ? null : kpp.Trim(),
                ContactNumber = phone.Trim(),
                Email = email.Trim(),
                Login = login.Trim(),
                Post = string.IsNullOrWhiteSpace(post) ? null : post.Trim(),
            };

            user.Password = hasher.HashPassword(user, password);

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.IdUser);

            return RedirectToPage("/Profile");
        }
    }
}
