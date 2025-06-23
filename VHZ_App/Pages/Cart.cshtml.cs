using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class CartModel : PageModel
    {
        private readonly VhzContext _context;

        public CartModel(VhzContext context)
        {
            _context = context;
        }
        public List<Cart> CartProducts { get; set; }
        public List<Cart> OrderProduct { get; set; }

        public decimal price { get; set; } = 0;
        public int amount { get; set; } = 0;

        public void OnGet()
        {
            CartProducts = _context.Carts.Include(c=>c.IdProductNavigation).ToList();
            amount = CartProducts.Count();
            foreach (var cart in CartProducts)
            {
                price += cart.IdProductNavigation.Price;
            }
        }

    }
}
