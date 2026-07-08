using E_Greetings.Models;

namespace E_Greetings.Models
{
    public class UserRoleViewModel
    {
        public User User { get; set; } = new User();
        public List<string> Roles { get; set; } = new List<string>();
    }
}