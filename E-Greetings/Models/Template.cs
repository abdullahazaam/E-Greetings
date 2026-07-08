// Models/Template.cs
namespace E_Greetings.Models
{
    public class Template
    {
        public int TemplateId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;         // ✅ Default value
        public string Description { get; set; } = string.Empty;   // ✅ Default value
        public string ImagePath { get; set; } = string.Empty;     // ✅ Default value
        public string? VideoPath { get; set; }                    // ✅ Nullable
        public string BackgroundColor { get; set; } = "#FFFFFF";  // ✅ Default value
        public string FontStyle { get; set; } = "Arial";          // ✅ Default value
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Category? Category { get; set; }                   // ✅ Nullable
        public ICollection<Card>? Cards { get; set; }             // ✅ Nullable
    }
}