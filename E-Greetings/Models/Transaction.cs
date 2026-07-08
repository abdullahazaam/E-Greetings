// Models/Transaction.cs
namespace E_Greetings.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CardId { get; set; }
        public string SenderId { get; set; } = string.Empty;      // ✅ Default value
        public string RecipientEmail { get; set; } = string.Empty; // ✅ Default value
        public DateTime SentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";           // ✅ Default value
        public string? ErrorMessage { get; set; }                 // ✅ Nullable

        public Card? Card { get; set; }                            // ✅ Nullable
        public User? Sender { get; set; }                          // ✅ Nullable
    }
}