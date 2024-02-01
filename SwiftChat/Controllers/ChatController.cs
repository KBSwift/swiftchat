using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwiftChat.Data;
using SwiftChat.Models.Entities;

namespace SwiftChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Redirect to login page in case user is not logged in and attempts to access
                return RedirectToAction("Login", "Account");
            }

            // Pass user's information to chat view
            return View(user);
        }
    }
}
