using System;
using System.Collections.Generic;

namespace VHZ_App.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Pathronimic { get; set; }

    public string? NameCompany { get; set; }

    public string Inn { get; set; } = null!;

    public string? Kpp { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Post { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
