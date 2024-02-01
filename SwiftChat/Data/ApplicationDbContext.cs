using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SwiftChat.Models.Entities;

namespace SwiftChat.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// many-to-many for saved messages
			modelBuilder.Entity<ChatMessage>()
				.HasMany(m => m.SavedByUsers)
				.WithMany(u => u.SavedMessages)
				.UsingEntity(j => j.ToTable("SavedMessages")); // Joining here

			// one-to-many for sender
			modelBuilder.Entity<ChatMessage>()
				.HasOne(m => m.Sender)
				.WithMany(u => u.SentMessages)
				.HasForeignKey(m => m.SenderId)
				.IsRequired(false); // Can be null
		}



	}
}
