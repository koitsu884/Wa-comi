using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class DailyTopicCreationDto
    {
        [Required]
        public string UserId{ get; set;}
        [Required]
        [MaxLength(100)]
        public string Title{ get; set;}

    }
}