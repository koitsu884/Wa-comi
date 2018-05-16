using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class UserRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set;} = "Member";
        public string DisplayName { get; set;}
    }
}