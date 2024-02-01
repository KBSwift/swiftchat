using System;

namespace SwiftChat.Models.Entities
{
	public class ChatMessage
	{
		public int Id { get; set; }
		public string? SenderId { get; set; } // Foreign key linking to ApplicationUser
		public string Message { get; set; } = string.Empty;
		public DateTime Timestamp { get; set; }
		public int Upvotes { get; set; } = 0;
		public int Downvotes { get; set; } = 0;
		public ICollection<ApplicationUser> SavedByUsers { get; set; } = new List<ApplicationUser>();

		// Navigation to ApplicationUser for easy referencing later!
		public virtual ApplicationUser? Sender { get; set; } // Avoided null with null reference check. virtual for lazy loading. Might help
	}
}
