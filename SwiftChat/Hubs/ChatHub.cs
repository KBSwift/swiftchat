using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SwiftChat.Data;
using SwiftChat.Models;
using Microsoft.AspNetCore.Identity;
using System;
using SwiftChat.Models.Entities;

namespace SwiftChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Inject ApplicationDbContext and UserManager into the ChatHub
        public ChatHub(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string message)
        {
            var userId = Context.UserIdentifier;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return;
            }

            var chatMessage = new ChatMessage
            {
                Message = message,
                Timestamp = DateTime.UtcNow,
                SenderId = userId
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Broadcast the message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user.UserName, message);
        }

    }
}