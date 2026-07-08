using E_Greetings.Data;
using E_Greetings.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace E_Greetings.Services
{
    public class ScheduledEmailService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledEmailService> _logger;

        public ScheduledEmailService(IServiceProvider serviceProvider, ILogger<ScheduledEmailService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessScheduledEmails();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing scheduled emails");
                }

                // ✅ Har 1 minute baad check karein
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessScheduledEmails()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var now = DateTime.Now;

            // ✅ Scheduled cards jo send hone wali hain
            var scheduledCards = await context.Cards
                .Include(c => c.Template)
                .Include(c => c.Sender)
                .Where(c => c.IsScheduled && !c.IsSent && c.ScheduleDate <= now)
                .ToListAsync();

            foreach (var card in scheduledCards)
            {
                try
                {
                    var cardImageUrl = !string.IsNullOrEmpty(card.PhotoPath)
                        ? card.PhotoPath
                        : card.Template?.ImagePath ??
                          "https://via.placeholder.com/600x400/6C63FF/FFFFFF?text=💌+Greeting+Card";

                    var senderName = card.Sender?.FullName ?? "A friend";
                    var recipientName = !string.IsNullOrEmpty(card.RecipientName) ? card.RecipientName : "Friend";

                    // ✅ EMAIL SEND KAREIN
                    var emailSent = await emailService.SendCardEmailAsync(
                        card.RecipientEmail,
                        card.Subject ?? "You've received a greeting card!",
                        card.Message ?? "A beautiful card from E-Greetings",
                        cardImageUrl,
                        recipientName,
                        senderName
                    );

                    card.Status = emailSent ? "Delivered" : "Failed";
                    card.IsSent = true;
                    card.SentDate = DateTime.Now;

                    _logger.LogInformation($"✅ Scheduled card {card.CardId} sent to {card.RecipientEmail}");

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ Failed to send scheduled card {card.CardId}");
                }
            }
        }
    }
}