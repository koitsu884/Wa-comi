using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public string UserId{get;set;}
        [Required]
        public string CurrentPassword{get;set;}
        [Required]
        public string NewPassword{get; set;}
    }
}