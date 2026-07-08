using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Greetings.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public FeedbackController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===== CREATE FEEDBACK =====
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                feedback.UserId = user.Id;
                feedback.SentDate = DateTime.Now;
                feedback.Status = "New";

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thank you for your feedback!";
                return RedirectToAction("Create");
            }

            return View(feedback);
        }
    }
}