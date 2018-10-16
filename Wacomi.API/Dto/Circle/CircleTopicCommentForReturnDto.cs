using System;

namespace Wacomi.API.Dto
{
    public class CircleTopicCommentForReturnDto
    {
        public int Id { get; set;}
        public int CircleTopicId{ get; set;}
        public string Comment{ get; set;}
        public PhotoForReturnDto Photo { get;set;}
        public int? PhotoId {get; set;}
        public int ReplyCount{ get; set;}
        public AppUserForReturnDto AppUser {get;set;}
        public int? AppUserId {get;set;}
        public DateTime DateCreated {get; set;}
        public DateTime DateUpdated {get; set;}
    }
}