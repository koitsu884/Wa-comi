using System;

namespace Wacomi.API.Dto.Circle
{
    public class CircleEventCommentForReturnDto
    {
         public int Id { get; set;}
        public int CircleEventId{ get; set;}
        public string Comment{ get; set;}
        public int ReplyCount{ get; set;}
        public AppUserForReturnDto AppUser {get;set;}
        public int? AppUserId {get;set;}
        public DateTime DateCreated {get; set;}
        public DateTime DateUpdated {get; set;}
    }
}