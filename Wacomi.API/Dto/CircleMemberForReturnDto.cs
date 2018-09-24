using System;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleMemberForReturnDto
    {
        public CircleForReturnDto Circle{get; set;}
        public int CircleId{ get; set;}
        public AppUserForReturnDto AppUser { get; set;}
        public int AppUserId { get; set;}
        public CircleRoleEnum Role { get; set;}
        public int? ApprovedBy{ get; set;}
        public DateTime DateJoined{ get; set;}
        public DateTime DateLastActive{ get; set;}
    }
}