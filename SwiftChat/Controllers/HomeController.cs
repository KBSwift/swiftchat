using Microsoft.AspNetCore.Mvc;
using SwiftChat.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SwiftChat.Models.Entities;

namespace SwiftChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager; // Add UserManager for user-specific logic

        // Inject UserManager into the constructor
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager; // Initialize UserManager
        }

        [Route("home/")]
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogError("User not found for UserHome page.");
                    return NotFound("User not found");
                }

                ViewBag.Username = user.UserName;
                if (TempData["SuccessMessage"] != null)
                {
                    ViewBag.SuccessMessage = TempData["SuccessMessage"];
                }
                return View("UserHome"); // Render UserHome view
            }
            else
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                return View(); // Render public home view (index)
            }
        }

        /*public IActionResult Index()
        {
            // Initially for logging in, now using for logout 
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            return View();
        }


        [Authorize]
        public async Task<IActionResult> UserHome()
        {
            // Retrieving current user if logged in
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handling error if user is not found
                _logger.LogError("User not found for UserHome page.");
                return NotFound("User not found");
            }

            // Adding user data to viewbag for view usage
            ViewBag.Username = user.UserName;

            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            return View();
        }*/

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
