using System;

namespace Wacomi.API.Dto
{
    public class CircleTopicForReturnDto
    {
        public int Id {get; set;}
        // public CircleForReturnDto Circle{get; set;}
        public int CircleId{ get; set;}
        public int TopicCommentCounts {get; set;}
        public bool IsSecret {get; set;}
        public string Title{get; set;}
        public string Description{ get; set;}
        public bool IsActive {get; set;}
        public DateTime DateCreated {get; set;}
        public DateTime DateUpdated {get; set;}
        public AppUserForReturnDto AppUser {get; set;}
        public int? AppUserId {get; set;}
        public PhotoForReturnDto Photo {get; set;}
        public int? PhotoId {get; set;}
    }
}