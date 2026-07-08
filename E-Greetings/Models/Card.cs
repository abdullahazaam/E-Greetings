using System.ComponentModel.DataAnnotations;

namespace E_Greetings.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public int TemplateId { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? PhotoPath { get; set; }
        public string? VideoPath { get; set; }
        public DateTime SentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";

        // ✅ NEW: Scheduling Properties
        public DateTime? ScheduleDate { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsSent { get; set; }

        // Navigation Properties
        public Template? Template { get; set; }
        public User? Sender { get; set; }
        public Transaction? Transaction { get; set; }
    }
}