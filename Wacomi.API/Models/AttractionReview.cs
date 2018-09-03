using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class AttractionReview
    {
        public int Id { get; set;}
        public AppUser AppUser{ get; set;}
        public int? AppUserId{ get; set;}
        public Attraction Attraction{ get; set;}
        [Required]
        public int AttractionId { get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto{ get; set;}
        public int? MainPhotoId{ get; set;}
        public virtual ICollection<AttractionReviewLike> AttractionReviewLikes { get; set;}
        [MaxLength(5)]
        public int Score{get; set;}
    }
}