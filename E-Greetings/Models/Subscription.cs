// Models/Subscription.cs
namespace E_Greetings.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string UserId { get; set; } = string.Empty;        // ✅ Default value
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; } = 9.99M;
        public string PaymentStatus { get; set; } = "Unpaid";     // ✅ Default value
        public string PaymentMethod { get; set; } = "Card";       // ✅ Default value
        public string EmailsList { get; set; } = string.Empty;    // ✅ Default value
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User? User { get; set; }                            // ✅ Nullable
    }
}