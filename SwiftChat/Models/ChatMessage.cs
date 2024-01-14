using System;

namespace SwiftChat.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty; // avoiding null values
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        // will tie user chat history in next db migration
        /*public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }*/


    }
}
