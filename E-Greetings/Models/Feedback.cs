// Models/Feedback.cs
namespace E_Greetings.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string UserId { get; set; } = string.Empty;        // ✅ Default value
        public string Message { get; set; } = string.Empty;       // ✅ Default value
        public int Rating { get; set; } = 5;                      // ✅ Default value
        public DateTime SentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "New";               // ✅ Default value

        public User? User { get; set; }                            // ✅ Nullable
    }
}