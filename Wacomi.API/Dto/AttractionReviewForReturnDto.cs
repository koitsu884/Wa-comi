using System;
using System.Collections.Generic;

namespace Wacomi.API.Dto
{
    public class AttractionReviewForReturnDto
    {
        public int Id { get; set;}
        public int AppUserId{ get; set;}
        public string AppUserName{ get; set;}
        public string AppUserIconUrl{ get; set;}
        public int AttractionId { get; set;}
        public string AttractionName { get; set;}
        public string AttractionThumbnailUrl { get; set;}
        public string CityName { get; set;}
        public virtual ICollection<PhotoForReturnDto> Photos { get; set; }
        public int? MainPhotoId{ get; set;}
        public PhotoForReturnDto MainPhoto { get; set;}
    //    public string MainPhotoUrl{ get; set;}
        public int LikedCount { get; set;}
        public bool IsLiked { get; set;}
        public int Score{get; set;}
        public string Review{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}