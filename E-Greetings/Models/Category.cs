// Models/Category.cs
namespace E_Greetings.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;          // ✅ Default value
        public string Description { get; set; } = string.Empty;   // ✅ Default value
        public string IconClass { get; set; } = "fa-gift";        // ✅ Default value
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Template>? Templates { get; set; }     // ✅ Nullable
    }
}