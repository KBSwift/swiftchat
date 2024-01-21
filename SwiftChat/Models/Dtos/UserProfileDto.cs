namespace SwiftChat.Models.Dtos
{
    public class UserProfileDto
    {
        public string? UserName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? Bio { get; set; }

        public DateTime? DateOfBirth { get; set; } = DateTime.MinValue;
        public string? Email { get; set; }

        // email and password update. Handling with CoreIdentity. Will place logic in controller
        public string? NewEmail { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
