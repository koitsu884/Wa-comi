using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class CircleTopicUpdateDto
    {
        public int Id {get; set;}
        [Required]
        public int CircleId{ get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public bool IsSecret {get; set;} = false;
        [Required]
        [MaxLength(StaticData.ShortTextLength)]
        public string Title{get; set;}
        [Required]
        [MaxLength(StaticData.LongTextLength)]
        public string Description{ get; set;}
        public bool IsActive {get; set;} = true;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}