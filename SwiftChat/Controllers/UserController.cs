using Microsoft.AspNetCore.Mvc;

namespace SwiftChat.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
			if (TempData["SuccessMessage"] != null)
			{
				ViewBag.SuccessMessage = TempData["SuccessMessage"];
			}
			return View();
        }
    }
}
