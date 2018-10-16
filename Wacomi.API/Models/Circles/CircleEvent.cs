using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleEvent : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id { get; set;}
        public int CircleId{ get; set;}
        [MaxLength(StaticData.ShortTextLength)]
        public string Title { get; set;}
        [MaxLength(StaticData.LongTextLength)]
        public string Description { get; set;}
        public bool IsPublic{ get; set;} = false;
        public string WebSiteUrls { get; set;}
        public int? MaxNumber{ get; set;}
        public bool ApprovalRequired{ get; set;} = false;
        public City City{ get; set;}
        public string Address{ get; set;}
        public int CityId{ get; set;}

        public ICollection<Photo> Photos {get; set;}
        public Photo MainPhoto {get; set;}
        public int? MainPhotoId {get; set;}
        public bool IsActive {get; set;}
        public DateTime DateCreated {get; set;}
        public DateTime DateUpdated {get; set;}
        public AppUser AppUser {get; set;}
        public int? AppUserId {get; set;}
        public virtual ICollection<CircleEventParticipation> CircleEventParticipations {get; set;}
    }
}