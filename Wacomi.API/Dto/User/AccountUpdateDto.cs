using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class AccountUpdateDto
    {
        [Required]
        public string UserName {get; set;}
        [Required]
        [EmailAddress]
        public string Email{ get; set;}
        // [Required]
        // public string DisplayName { get; set;}
    }
}