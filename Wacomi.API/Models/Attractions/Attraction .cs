using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public class Attraction : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id {get; set;}
        public bool IsActive{get; set;} = true;
        public AppUser AppUser{ get; set;}
        public int? AppUserId { get; set;}
        public City City{ get; set;}
        [Required]
        public int CityId { get; set;}
        [Required]
        [MaxLength(100)]
        public string Name{ get; set;}
        [Required]
        [MaxLength(1500)]
        public string Introduction {get; set;}
        public string AccessInfo{ get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;}
        [MaxLength(5000)]
        public int? Radius{ get; set;}
        public virtual ICollection<AttractionLike> AttractionLikes { get; set;}
        public virtual ICollection<AttractionReview> AttractionReviews { get; set;}
        public virtual ICollection<AttractionCategorization> Categorizations  { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto{ get; set;}
        public int? MainPhotoId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}