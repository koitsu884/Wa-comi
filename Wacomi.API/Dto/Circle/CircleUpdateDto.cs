using System;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleUpdateDto
    {
        public int? Id { get; set;}
        [Required]
        [MaxLength(StaticData.ShortTextLength)]
        public string Name{ get; set;}
        [Required]
        public int CategoryId { get; set;}
        public int? CityId{ get; set;}
        [Required]
        [MaxLength(StaticData.LongTextLength)]
        public string Description{ get; set;}
        public bool ApprovalRequired { get; set;} = false;
        [Required]
        public int AppUserId{ get; set;}
        public int? MainPhotoId { get; set;}
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}