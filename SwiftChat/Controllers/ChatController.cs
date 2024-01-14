using Microsoft.AspNetCore.Mvc;

namespace SwiftChat.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat/Index. Testing
        public IActionResult Index()
        {

            return View();
        }

        // Probably will add actions related to chat here (like retrieving chat history)
    }
}