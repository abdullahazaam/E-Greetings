using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using E_Greetings.Models;

namespace E_Greetings.Filters
{
    public class ActiveUserFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ActiveUserFilter(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(user);
                if (!string.IsNullOrEmpty(userId))
                {
                    var appUser = await _userManager.FindByIdAsync(userId);

                    if (appUser == null || !appUser.IsActive)  // ✅ IsActive use karein
                    {
                        await _signInManager.SignOutAsync();
                        context.Result = new RedirectToActionResult("Login", "Account", new { message = "Your account has been deactivated." });
                        return;
                    }
                }
            }
        }
    }
}