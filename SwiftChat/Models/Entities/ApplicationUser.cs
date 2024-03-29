﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SwiftChat.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfilePicture { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
		public ICollection<ChatMessage> SavedMessages { get; set; } = new List<ChatMessage>();
		public ICollection<ChatMessage> SentMessages { get; set; } = new List<ChatMessage>();
	}

}