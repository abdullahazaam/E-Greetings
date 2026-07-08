// Models/User.cs
using Microsoft.AspNetCore.Identity;

namespace E_Greetings.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePhoto { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public string? Role { get; set; }

        // Navigation properties
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public Subscription? Subscription { get; set; }
    }
}