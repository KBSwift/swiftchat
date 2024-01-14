using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SwiftChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicturePath { get; set; } = string.Empty;

        [StringLength(500)]
        public string Bio { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }

}
