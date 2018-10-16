using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class AttractionReviewUpdateDto
    {
        public int Id { get; set;}
        [Required]
        public int AttractionId{ get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public int? MainPhotoId{ get; set;}
        public int Score{get; set;}
        [Required]
        [MaxLength(StaticData.LongTextLength)]
        public string Review{ get; set;}
    }
}