using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class AttractionForReturnDto
    {
        public int Id {get; set;}
        public AppUserForReturnDto AppUser{ get; set;}
        public string CityName{ get; set;}
        public string CityId{ get; set;}
        public string Name{ get; set;}
        public string Introduction {get; set;}
        public string AccessInfo{ get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;}
        public int? Radius{ get; set;}
        public ICollection<PhotoForReturnDto> Photos { get; set; }
        public int LikedCount{ get; set;}
        public bool isLiked{ get; set;} = false;
        public ICollection<AttractionReviewForReturnDto> AttractionReviews { get; set; }
        
        public ICollection<PhotoForReturnDto> ReviewPhotos { get; set; }
        public int ReviewedCount { get; set;}
        public int currentUsersReviewId { get; set;}
        public double ScoreAverage{ get; set;}
        public ICollection<CategoryForReturnDto> Categories { get; set; }
        //public string MainPhotoUrl { get; set;}
        public PhotoForReturnDto MainPhoto { get; set;}
        public int? MainPhotoId { get; set;}
        public string WebsiteUrl{ get; set;}
        public DateTime DateCreated{ get; set;}
        public DateTime DateUpdated{ get; set;}
    }
}