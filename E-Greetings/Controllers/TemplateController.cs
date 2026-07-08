using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Controllers
{
    public class TemplateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== ALL TEMPLATES (with filtering) =====
        public async Task<IActionResult> Index(int? categoryId)
        {
            var templates = _context.Templates.Include(t => t.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                templates = templates.Where(t => t.CategoryId == categoryId.Value);
            }

            ViewBag.SelectedCategory = categoryId;
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(await templates.ToListAsync());
        }

        // ===== TEMPLATE DETAILS =====
        public async Task<IActionResult> Details(int id)
        {
            var template = await _context.Templates
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TemplateId == id);

            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }
    }
}