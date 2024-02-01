using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SwiftChat.Data;
using SwiftChat.Hubs;
using SwiftChat.Models.Entities;

namespace SwiftChat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		private readonly IHubContext<ChatHub> _hubContext;

		public ChatController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
			_context = context;
			_hubContext = hubContext;
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

		[HttpPost]
		public async Task<IActionResult> UpvoteMessage(int messageId)
		{
			var message = await _context.ChatMessages.FindAsync(messageId);
			if (message == null)
			{
				return NotFound();
			}

			message.Upvotes += 1;
			await _context.SaveChangesAsync();
			await _hubContext.Clients.All.SendAsync("ReceiveMessageUpdate", message.Id, message.Upvotes, message.Downvotes, message.SavedByUsers.Count);

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> DownvoteMessage(int messageId)
		{
			var message = await _context.ChatMessages.FindAsync(messageId);
			if (message == null)
			{
				return NotFound();
			}

			message.Downvotes += 1;
			await _context.SaveChangesAsync();
			await _hubContext.Clients.All.SendAsync("ReceiveMessageUpdate", message.Id, message.Upvotes, message.Downvotes, message.SavedByUsers.Count);

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> SaveMessage(int messageId)
		{
			var message = await _context.ChatMessages
				.Include(m => m.SavedByUsers) // Including to load collection
				.FirstOrDefaultAsync(m => m.Id == messageId);
			var user = await _userManager.GetUserAsync(User);
			if (message == null || user == null)
			{
				return NotFound();
			}

			if (!message.SavedByUsers.Contains(user))
			{
				message.SavedByUsers.Add(user);
				await _context.SaveChangesAsync();
			}

			await _hubContext.Clients.All.SendAsync("ReceiveMessageUpdate", message.Id, message.Upvotes, message.Downvotes, message.SavedByUsers.Count);
			return Ok();
		}

	}
}
