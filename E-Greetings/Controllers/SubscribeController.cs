using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Greetings.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public SubscribeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===== PLANS =====
        public IActionResult Plans()
        {
            return View();
        }

        // ===== PAYMENT =====
        [Authorize]
        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(Subscription model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var subscription = new Subscription
                {
                    UserId = user.Id,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    Amount = model.Amount > 0 ? model.Amount : 9.99m,
                    PaymentStatus = "Paid",
                    PaymentMethod = model.PaymentMethod,
                    EmailsList = model.EmailsList,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Subscription activated successfully!";
                return RedirectToAction("Confirmation");
            }

            return View(model);
        }

        // ===== CONFIRMATION =====
        public IActionResult Confirmation()
        {
            return View();
        }

        // ===== MY SUBSCRIPTION =====
        [Authorize]
        public async Task<IActionResult> MySubscription()
        {
            var user = await _userManager.GetUserAsync(User);
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == user.Id && s.IsActive);

            return View(subscription);
        }
    }
}