namespace E_Greetings.Models
{
    public class ReportsViewModel
    {
        public int TodayCards { get; set; }
        public int WeekCards { get; set; }
        public int MonthCards { get; set; }
        public List<CategoryCardCount> CardsByCategory { get; set; } = new List<CategoryCardCount>();
        public List<DailyTransaction> DailyTransactions { get; set; } = new List<DailyTransaction>();
    }

    public class CategoryCardCount
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class DailyTransaction
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}