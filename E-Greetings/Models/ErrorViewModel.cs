// Models/ErrorViewModel.cs
namespace E_Greetings.Models   // ✅ E_Greetings (project ke hisaab se)
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}