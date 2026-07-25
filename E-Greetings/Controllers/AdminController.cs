using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ===== DASHBOARD =====
        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalTemplates = await _context.Templates.CountAsync(),
                TotalCardsSent = await _context.Cards.CountAsync(),
                TotalFeedbacks = await _context.Feedbacks.CountAsync(),
                RecentCards = await _context.Cards
                    .Include(c => c.Template)
                    .Include(c => c.Sender)
                    .OrderByDescending(c => c.SentDate)
                    .Take(10)
                    .ToListAsync(),
                RecentFeedbacks = await _context.Feedbacks
                    .Include(f => f.User)
                    .OrderByDescending(f => f.SentDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        // ===== TEMPLATES MANAGEMENT =====
        public async Task<IActionResult> Templates()
        {
            var templates = await _context.Templates
                .Include(t => t.Category)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();
            return View(templates);
        }

        [HttpGet]
        public async Task<IActionResult> AddTemplate()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTemplate(Template template)
        {
            if (ModelState.IsValid)
            {
                template.CreatedDate = DateTime.Now;
                _context.Add(template);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Template added successfully!";
                return RedirectToAction(nameof(Templates));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(template);
        }

        [HttpGet]
        public async Task<IActionResult> EditTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTemplate(int id, Template template)
        {
            if (id != template.TemplateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(template);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Template updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateExists(template.TemplateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Templates));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(template);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template != null)
            {
                _context.Templates.Remove(template);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Template deleted successfully!";
            }
            return RedirectToAction(nameof(Templates));
        }

        // ===== FEEDBACKS =====
        public async Task<IActionResult> Feedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .OrderByDescending(f => f.SentDate)
                .ToListAsync();
            return View(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Feedback deleted successfully!";
            }
            return RedirectToAction(nameof(Feedbacks));
        }

        // ===== TRANSACTIONS =====
        public async Task<IActionResult> Transactions()
        {
            var transactions = await _context.Cards
                .Include(c => c.Template)
                .Include(c => c.Sender)
                .OrderByDescending(c => c.SentDate)
                .ToListAsync();
            return View(transactions);
        }

        // ===== USERS MANAGEMENT =====
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    User = user,
                    Roles = roles.ToList()
                });
            }

            return View(userRoles);
        }

        // ===== TOGGLE USER STATUS (ACTIVATE/DEACTIVATE) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction(nameof(Users));
            }

            // ✅ Admin khud ko deactivate nahi kar sakta
            if (user.Email == "admin@egreetings.com")
            {
                TempData["Error"] = "You cannot deactivate the main admin account!";
                return RedirectToAction(nameof(Users));
            }

            // ✅ Toggle status
            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);

            // ✅ AGAR DEACTIVATE KIYA HAI TO FORCE LOGOUT
            if (!user.IsActive)
            {
                // ✅ Forcefully logout agar currently logged in hai
                await _signInManager.SignOutAsync();
                TempData["Success"] = $"User {user.FullName} has been deactivated and logged out!";
            }
            else
            {
                TempData["Success"] = $"User {user.FullName} has been activated!";
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    TempData["Success"] = "User promoted to Admin!";
                }
                else
                {
                    TempData["Error"] = "User is already an Admin!";
                }
            }
            return RedirectToAction(nameof(Users));
        }

        // ===== DELETE USER =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction(nameof(Users));
            }

            // Admin delete nahi kar sakta khud ko
            if (user.Email == "admin@egreetings.com")
            {
                TempData["Error"] = "Cannot delete the main admin account!";
                return RedirectToAction(nameof(Users));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "User deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete user!";
            }

            return RedirectToAction(nameof(Users));
        }

        // ===== CHANGE USER ROLE =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction(nameof(Users));
            }

            // Admin role nahi change kar sakte
            if (user.Email == "admin@egreetings.com")
            {
                TempData["Error"] = "Cannot change the main admin role!";
                return RedirectToAction(nameof(Users));
            }

            // Current roles remove karein
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // New role add karein
            await _userManager.AddToRoleAsync(user, role);

            TempData["Success"] = $"User role changed to {role} successfully!";
            return RedirectToAction(nameof(Users));
        }

        // ===== REPORTS =====
        public async Task<IActionResult> Reports()
        {
            var today = DateTime.Now.Date;
            var model = new ReportsViewModel
            {
                TodayCards = await _context.Cards.CountAsync(c => c.SentDate.Date == today),
                WeekCards = await _context.Cards.CountAsync(c => c.SentDate.Date >= today.AddDays(-7)),
                MonthCards = await _context.Cards.CountAsync(c => c.SentDate.Date >= today.AddMonths(-1)),
                CardsByCategory = await _context.Cards
                    .Include(c => c.Template)
                    .GroupBy(c => c.Template.CategoryId)
                    .Select(g => new CategoryCardCount
                    {
                        CategoryId = g.Key,
                        Count = g.Count(),
                        CategoryName = g.First().Template.Category.Name
                    })
                    .ToListAsync(),
                DailyTransactions = await _context.Cards
                    .GroupBy(c => c.SentDate.Date)
                    .Select(g => new DailyTransaction
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(d => d.Date)
                    .Take(30)
                    .ToListAsync()
            };

            return View(model);
        }

        private bool TemplateExists(int id)
        {
            return _context.Templates.Any(e => e.TemplateId == id);
        }
    }
}