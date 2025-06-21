using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using VHZ_App.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace VHZ_App.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly VhzContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EditProfileModel> _logger;

        public EditProfileModel(VhzContext context, IWebHostEnvironment environment, ILogger<EditProfileModel> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public IFormFile? AvatarFile { get; set; } // ������ nullable � ��������������

        public string CurrentAvatarPath { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "������� �����������")]
            [Display(Name = "�������")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "��� �����������")]
            [Display(Name = "���")]
            public string Name { get; set; }

            [Display(Name = "��������")]
            public string? Pathronimic { get; set; }

            [Required(ErrorMessage = "������� ����������")]
            [Display(Name = "�������")]
            [Phone(ErrorMessage = "������������ ����� ��������")]
            public string ContactNumber { get; set; }

            [Required(ErrorMessage = "Email ����������")]
            [Display(Name = "Email")]
            [EmailAddress(ErrorMessage = "������������ email")]
            public string Email { get; set; }

            [Display(Name = "�������� ��������")]
            public string? NameCompany { get; set; }

            [Display(Name = "���������")]
            public string? Post { get; set; }

            [Required(ErrorMessage = "��� ����������")]
            [Display(Name = "���")]
            [StringLength(12, MinimumLength = 10, ErrorMessage = "��� ������ ��������� 10 ��� 12 ����")]
            [RegularExpression(@"^\d+$", ErrorMessage = "��� ������ ��������� ������ �����")]
            public string Inn { get; set; }

            [Display(Name = "���")]
            [StringLength(9, MinimumLength = 9, ErrorMessage = "��� ������ ��������� 9 ���� (���� ������)")]
            [RegularExpression(@"^\d+$", ErrorMessage = "��� ������ ��������� ������ �����")]
            public string? Kpp { get; set; }
        }

        public IActionResult OnGet()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    _logger.LogWarning("������������ �� �����������");
                    return RedirectToPage("/Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.IdUser == userId);
                if (user == null)
                {
                    _logger.LogWarning($"������������ � ID {userId} �� ������");
                    return RedirectToPage("/Account");
                }

                Input = new InputModel
                {
                    Surname = user.Surname,
                    Name = user.Name,
                    Pathronimic = user.Pathronimic,
                    ContactNumber = user.ContactNumber,
                    Email = user.Email,
                    NameCompany = user.NameCompany,
                    Post = user.Post,
                    Inn = user.Inn,
                    Kpp = user.Kpp
                };

                CurrentAvatarPath = GetAvatarPath(user.IdUser);
                _logger.LogInformation($"������� ������: {CurrentAvatarPath}");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ � ������ OnGet");
                return RedirectToPage("/Error", new { message = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("������ ��������� POST �������");

                // ������� ������ ��������� ��� AvatarFile, ���� ��� ����
                ModelState.Remove("AvatarFile");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("������ �� �������");
                    var userIdValidation = HttpContext.Session.GetInt32("UserId");
                    if (userIdValidation.HasValue)
                    {
                        CurrentAvatarPath = GetAvatarPath(userIdValidation.Value);
                    }
                    return Page();
                }

                var userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    _logger.LogWarning("������ �� �������� UserId");
                    return RedirectToPage("/Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.IdUser == userId);
                if (user == null)
                {
                    _logger.LogWarning($"������������ � ID {userId} �� ������");
                    return RedirectToPage("/Account");
                }

                // ���������� ������ ������������
                user.Surname = Input.Surname;
                user.Name = Input.Name;
                user.Pathronimic = Input.Pathronimic;
                user.ContactNumber = Input.ContactNumber;
                user.Email = Input.Email;
                user.NameCompany = Input.NameCompany;
                user.Post = Input.Post;
                user.Inn = Input.Inn;
                user.Kpp = Input.Kpp;

                // ��������� ������� (��������������)
                if (AvatarFile != null && AvatarFile.Length > 0)
                {
                    try
                    {
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                            _logger.LogInformation($"������� �����: {uploadsFolder}");
                        }

                        var fileExtension = Path.GetExtension(AvatarFile.FileName).ToLower();
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("AvatarFile", "��������� ������ JPG, JPEG ��� PNG");
                            return Page();
                        }

                        var fileName = $"{userId}{fileExtension}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // �������� ������ ��������
                        foreach (var ext in allowedExtensions)
                        {
                            var oldPath = Path.Combine(uploadsFolder, $"{userId}{ext}");
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await AvatarFile.CopyToAsync(stream);
                        }

                        _logger.LogInformation($"������ ��������: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "������ ��� �������� �������");
                        ModelState.AddModelError("AvatarFile", "������ ��� �������� �����������");
                        return Page();
                    }
                }
                else
                {
                    _logger.LogInformation("������ �� ��� �������� - ��� ���������");
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("������ ������������ ���������");

                return RedirectToPage("/Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "����������� ������ � OnPostAsync");
                return RedirectToPage("/Error", new { message = ex.Message });
            }
        }

        private string GetAvatarPath(int userId)
        {
            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");

                foreach (var ext in allowedExtensions)
                {
                    var avatarPath = Path.Combine(uploadsFolder, $"{userId}{ext}");
                    if (System.IO.File.Exists(avatarPath))
                    {
                        return $"/uploads/avatars/{userId}{ext}";
                    }
                }

                return "/Image/user-placeholder.png";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "������ � GetAvatarPath");
                return "/Image/user-placeholder.png";
            }
        }
    }
}