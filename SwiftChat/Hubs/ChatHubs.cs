using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SwiftChat.Data;

namespace SwiftChat.Hubs
{
    public class ChatHub : Hub
    {


        public async Task SendMessage(string user, string message)
        {
            // This line sends the received message to all connected clients in real-time
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // WIP FOR CHAT HISTORY!!!
        /*private readonly ApplicationDbContext _context;

        // Constructor injection for ApplicationDbContext
        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message)
        {
            // Retrieve the username from the current context
            var username = Context?.User?.Identity?.Name;

            if (username == null)
            {
                // In case username is actually null
                return;
            }

            // Create and save the chat message
            var chatMessage = new ChatMessage;
            {
                Username = username,
                Message = message,
                Timestamp = DateTime.UtcNow // Store the timestamp
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Send the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }*/
    }
}
