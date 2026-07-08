using E_Greetings.Data;
using E_Greetings.Models;
using E_Greetings.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace E_Greetings.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public CardController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int templateId)
        {
            var template = await _context.Templates.FindAsync(templateId);
            if (template == null)
            {
                return NotFound();
            }

            ViewBag.Template = template;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Card card, string scheduleDate, string scheduleTime)
        {
            // ✅ ModelState validation - sirf mandatory fields check karega
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Model Error: {error.ErrorMessage}");
                }

                var templateData = await _context.Templates.FindAsync(card.TemplateId);
                ViewBag.Template = templateData;
                TempData["Error"] = "Please fill in all required fields.";
                return View(card);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                card.SenderId = user.Id;

                // ✅ SCHEDULE - COMPLETELY OPTIONAL
                bool isScheduled = false;

                if (!string.IsNullOrEmpty(scheduleDate) && !string.IsNullOrEmpty(scheduleTime))
                {
                    try
                    {
                        var scheduleDateTime = DateTime.ParseExact(
                            $"{scheduleDate} {scheduleTime}",
                            "yyyy-MM-dd HH:mm",
                            CultureInfo.InvariantCulture
                        );

                        if (scheduleDateTime > DateTime.Now)
                        {
                            card.ScheduleDate = scheduleDateTime;
                            card.IsScheduled = true;
                            card.IsSent = false;
                            card.Status = "Scheduled";
                            card.SentDate = scheduleDateTime;
                            isScheduled = true;
                        }
                        else
                        {
                            TempData["Error"] = "Schedule date must be in the future!";
                            var templateObj = await _context.Templates.FindAsync(card.TemplateId);
                            ViewBag.Template = templateObj;
                            return View(card);
                        }
                    }
                    catch (FormatException)
                    {
                        TempData["Error"] = "Invalid schedule date/time format! Use YYYY-MM-DD HH:MM";
                        var templateFormat = await _context.Templates.FindAsync(card.TemplateId);
                        ViewBag.Template = templateFormat;
                        return View(card);
                    }
                }

                // ✅ Agar schedule nahi hai to immediately send
                if (!isScheduled)
                {
                    card.SentDate = DateTime.Now;
                    card.Status = "Pending";
                    card.IsScheduled = false;
                    card.IsSent = false;
                    card.ScheduleDate = null;
                }

                _context.Add(card);
                await _context.SaveChangesAsync();

                // ✅ Agar schedule nahi hai to direct send page par bhejein
                return RedirectToAction("Send", new { id = card.CardId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                var templateEx = await _context.Templates.FindAsync(card.TemplateId);
                ViewBag.Template = templateEx;
                return View(card);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Send(int id)
        {
            var card = await _context.Cards
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id);

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(Card card)
        {
            var existingCard = await _context.Cards
                .Include(c => c.Template)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(c => c.CardId == card.CardId);

            if (existingCard == null)
            {
                return NotFound();
            }

            existingCard.Subject = card.Subject;
            existingCard.Message = card.Message;
            existingCard.RecipientEmail = card.RecipientEmail;
            existingCard.RecipientName = card.RecipientName;

            // ✅ IF SCHEDULED - Save and return
            if (existingCard.IsScheduled && existingCard.ScheduleDate.HasValue)
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = $"✅ Card scheduled for {existingCard.ScheduleDate.Value:dd MMM yyyy, hh:mm tt}!";
                return RedirectToAction("MyCards");
            }

            // ✅ NORMAL CARD - Send email immediately
            var cardImageUrl = !string.IsNullOrEmpty(existingCard.PhotoPath)
                ? existingCard.PhotoPath
                : existingCard.Template?.ImagePath ??
                  "https://via.placeholder.com/600x400/6C63FF/FFFFFF?text=💌+Greeting+Card";

            var senderName = existingCard.Sender?.FullName ?? "A friend";
            var recipientName = !string.IsNullOrEmpty(existingCard.RecipientName)
                ? existingCard.RecipientName
                : "Friend";

            try
            {
                var emailSent = await _emailService.SendCardEmailAsync(
                    existingCard.RecipientEmail,
                    card.Subject ?? "You've received a greeting card!",
                    existingCard.Message ?? "A beautiful card from E-Greetings",
                    cardImageUrl,
                    recipientName,
                    senderName
                );

                existingCard.Status = emailSent ? "Delivered" : "Failed";
                existingCard.IsSent = emailSent;
                await _context.SaveChangesAsync();

                if (!emailSent)
                {
                    TempData["Error"] = "Email could not be sent. Please try again.";
                    return RedirectToAction("Send", new { id = card.CardId });
                }

                return RedirectToAction("Success", new { id = card.CardId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Email error: {ex.Message}";
                return RedirectToAction("Send", new { id = card.CardId });
            }
        }

        public async Task<IActionResult> Success(int id)
        {
            var card = await _context.Cards
                .Include(c => c.Template)
                .FirstOrDefaultAsync(c => c.CardId == id);

            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        public async Task<IActionResult> MyCards()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cards = await _context.Cards
                .Include(c => c.Template)
                .Where(c => c.SenderId == user.Id)
                .OrderByDescending(c => c.SentDate)
                .ToListAsync();

            return View(cards);
        }

        [Authorize]
        public async Task<IActionResult> DownloadCard(int id)
        {
            try
            {
                var card = await _context.Cards
                    .Include(c => c.Template)
                    .Include(c => c.Sender)
                    .FirstOrDefaultAsync(c => c.CardId == id);

                if (card == null)
                {
                    return NotFound();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var hasSubscription = await _context.Subscriptions
                    .AnyAsync(s => s.UserId == user.Id && s.IsActive && s.EndDate > DateTime.Now);

                if (!hasSubscription)
                {
                    TempData["Error"] = "This feature is available for Premium users only. Please subscribe to download cards.";
                    return RedirectToAction("Plans", "Subscribe");
                }

                TempData["Success"] = "Card downloaded successfully!";
                return RedirectToAction("MyCards");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Download failed: {ex.Message}";
                return RedirectToAction("MyCards");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewCard(int id)
        {
            var card = await _context.Cards
                .Include(c => c.Template)
                .Include(c => c.Sender)
                .FirstOrDefaultAsync(c => c.CardId == id);

            if (card == null || card.Status != "Delivered")
            {
                return NotFound();
            }

            return View(card);
        }
    }
}