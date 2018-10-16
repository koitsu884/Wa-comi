using System;

namespace Wacomi.API.Models
{
    public enum CircleRoleEnum {
        OWNER,
        MEMBER
    }
    public class CircleMember
    {
        public Circle Circle{get; set;}
        public int CircleId{ get; set;}
        public AppUser AppUser { get; set;}
        public int AppUserId { get; set;}
        public CircleRoleEnum Role { get; set;}
        public int? ApprovedBy{ get; set;}
        public DateTime? DateJoined{ get; set;} = DateTime.Now;
        public DateTime? DateLastActive{ get; set;} = DateTime.Now;
    }
}