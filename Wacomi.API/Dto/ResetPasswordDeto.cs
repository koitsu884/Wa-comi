using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class ResetPasswordDeto
    {
        [Required]
        public string UserId{get;set;}
        [Required]
        public string Code{get;set;}
        [Required]
        public string Password{get; set;}
    }
}