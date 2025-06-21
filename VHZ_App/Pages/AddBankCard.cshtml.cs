using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHZ_App.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace VHZ_App.Pages
{
    public class AddBankCardModel : PageModel
    {
        private readonly VhzContext _context;
        private readonly string _encryptionKey;

        public AddBankCardModel(VhzContext context)
        {
            _context = context;
            _encryptionKey = "YourEncryptionKey123";
        }

        [BindProperty]
        [Required(ErrorMessage = "Номер карты обязателен")]
        [CreditCard(ErrorMessage = "Неверный номер карты")]
        public string CardNumber { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Срок действия обязателен")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Неверный формат срока действия")]
        public string ValidityPeriod { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "CVV/CVC обязателен")]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "CVV/CVC должен состоять из 3 цифр")]
        public string CvvCvc { get; set; }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var existingCard = _context.BankCards.FirstOrDefault(c => c.IdUser == userId);
            if (existingCard != null)
            {
                return RedirectToPage("/Profile");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var cleanCardNumber = CardNumber.Replace(" ", "");
            var encryptedCvv = EncryptString(CvvCvc, _encryptionKey);

            var newCard = new BankCard
            {
                IdUser = userId.Value,
                CardNumber = cleanCardNumber,
                ValidityPeriod = ValidityPeriod,
                CvvCvc = encryptedCvv // Сохраняем зашифрованный CVV
            };

            _context.BankCards.Add(newCard);
            _context.SaveChanges();

            return RedirectToPage("/Profile");
        }

        // Методы шифрования/дешифрования
        private string EncryptString(string text, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32)); // 256-bit key
                aesAlg.IV = new byte[16]; // 128-bit IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        private string DecryptString(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}