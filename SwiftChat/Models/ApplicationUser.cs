using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SwiftChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfilePicture { get; set; }

        [StringLength(500)]
        public string Bio { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }

}
