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

        // GET: Chat/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Redirect to login page or handle the case where the user is not logged in
                return RedirectToAction("Login", "Account");
            }

            // Pass the user's information to the view
            return View(user);
        }
    }
}
