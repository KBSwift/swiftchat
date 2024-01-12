// necessary directive for SignalR
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks; // Might not need

// Define the namespace to match with your project structure
namespace SwiftChat.Hubs
{
    // Defining the ChatHub class inside SwiftChat.Hubs namespace which is what I want
    public class ChatHub : Hub
    {
        // Asynchronous method to send a message to all connected clients
        public async Task SendMessage(string user, string message)
        {
            // This line sends the received message to all connected clients in real-time
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
