using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public class AttractionReview : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id { get; set;}
        public bool IsActive{ get; set;} = true;
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
        [MaxLength(1500)]
        [Required]
        public string Review{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}