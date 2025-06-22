using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VHZ_App.Models
{
    public partial class Contact
    {
        public int IdContact { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; } 

        [Display(Name = "Имя контакта")]
        public string? NameContact { get; set; }

        [Display(Name = "Телефон")]
        [Column(TypeName = "varchar(50)")]
        public string? NumberPhone { get; set; }
    }
}