using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== ALL CATEGORIES =====
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // ===== CATEGORY DETAILS (Templates in Category) =====
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Templates)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}