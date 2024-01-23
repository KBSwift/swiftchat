using System.ComponentModel.DataAnnotations;

namespace SwiftChat.Models.Dtos
{
    public class UserProfileDto
    {
        public string? UserName { get; set; }
        public byte[]? ProfilePicture { get; set; }

		[StringLength(500)]
		public string? Bio { get; set; }

        public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
        public string? Email { get; set; }

		// email and password update. Handling with CoreIdentity. Will place logic in controller
		[EmailAddress]
		[Display(Name = "Email")]
		public string? NewEmail { get; set; }

		[Required]
		public string? CurrentPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "new password")]
		[StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters.", MinimumLength = 6)]
		public string? NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
		public string? ConfirmNewPassword { get; set; }
	}
}
