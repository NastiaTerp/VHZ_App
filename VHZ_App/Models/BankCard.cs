using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace VHZ_App.Models;

public partial class BankCard
{
    public int IdBankCard { get; set; }

    public int IdUser { get; set; }

    public string ValidityPeriod { get; set; } = null!;

    [StringLength(50)]
    public string CvvCvc { get; set; } = null!;

    public string CardNumber { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    // Метод для безопасного получения CVV (только при необходимости)
    public string GetDecryptedCvv(string encryptionKey)
    {
        try
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32).Substring(0, 32));
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(this.CvvCvc)))
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
        catch
        {
            // В случае ошибки дешифрования возвращаем пустую строку
            return string.Empty;
        }
    }
}