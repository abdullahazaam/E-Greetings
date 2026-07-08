namespace E_Greetings.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalTemplates { get; set; }
        public int TotalCardsSent { get; set; }
        public int TotalFeedbacks { get; set; }
        public List<Card> RecentCards { get; set; } = new List<Card>();
        public List<Feedback> RecentFeedbacks { get; set; } = new List<Feedback>();
    }
}