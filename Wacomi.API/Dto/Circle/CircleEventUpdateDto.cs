using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class CircleEventUpdateDto
    {
        public int Id { get; set;}
        [Required]
        public int CircleId{ get; set;}
        [Required]
        [MaxLength(StaticData.ShortTextLength)]
        public string Title { get; set;}
        [Required]
        [MaxLength(StaticData.LongTextLength)]
        public string Description { get; set;}
        public bool IsPublic{ get; set;} = false;
        [Required]
        public DateTime FromDate { get; set;}
        public DateTime? ToDate{ get; set;}
        public string WebSiteUrls { get; set;}
        public int? MaxNumber{ get; set;}
        public bool ApprovalRequired{ get; set;} = false;
        public string Address{ get; set;}
        [Required]
        public int CityId{ get; set;}
        public int? MainPhotoId {get; set;}
        public bool IsActive {get; set;} = true;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
        [Required]
        public int AppUserId {get; set;}
    }
}