using System;
using System.ComponentModel.DataAnnotations;

namespace VHZ_App.Models
{
    public partial class Contact
    {
        public int IdContact { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }  // Изменено на nullable

        [Display(Name = "Имя контакта")]
        public string? NameContact { get; set; }  // Изменено на nullable

        [Display(Name = "Телефон")]
        public string? NumberPhone { get; set; }  // Изменено на nullable
    }
}