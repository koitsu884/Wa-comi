using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class EmailConfirmationDto
    {
        [Required]
        public string UserId {get; set;}
        [Required]
        public string Code { get; set;}
    }
}