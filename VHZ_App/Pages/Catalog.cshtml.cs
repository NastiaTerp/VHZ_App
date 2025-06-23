using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VHZ_App.Models;

namespace VHZ_App.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly VhzContext _context;

        public CatalogModel(VhzContext context)
        {
            _context = context;
        }
        public List<Product> CatalogProducts { get; set; }

        [BindProperty]
        public string foundPole { get; set; } = "";
        public IActionResult SetProductId(int idProduct)
        {
            HttpContext.Session.SetInt32("ProductId", idProduct); // Записываем ID
            return RedirectToAction("/CardProduct"); // Перенаправляем
        }
        public void OnGet()
        {
            CatalogProducts = _context.Products.OrderBy(p => p.Type).ToList();
        }
    }
}
