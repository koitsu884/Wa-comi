using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class DailyTopicCreationDto
    {
        [Required]
        public int UserId{ get; set;}
        [Required]
        [MaxLength(StaticData.ShortTextLength)]
        public string Title{ get; set;}

    }
}