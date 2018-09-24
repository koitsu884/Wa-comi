using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleForReturnDto
    {
        public int Id{ get; set;}
        public AppUserForReturnDto AppUser{ get; set;}
        public int AppUserId{ get; set;}
        public string Name{ get; set;}
        public string CategoryName { get; set;}
        public int CategoryId { get; set;}
        public string CityName { get; set;}
        public int? CityId { get; set;}
        public string Description{ get; set;}
        public bool ApprovalRequired { get; set;}
        public int TotalMemberCount{get; set;}
        // public ICollection<AppUserForListDto> CircleMemberList {get; set;}
        public virtual ICollection<PhotoForReturnDto> Photos{ get; set;}
        public virtual ICollection<CircleTopicForReturnDto> Topics{ get; set;}
        public PhotoForReturnDto MainPhoto { get; set;}
        public bool IsActive { get; set;}
        public DateTime DateCreated { get; set;}
        public DateTime DateUpdated { get; set;}

        public bool IsManageable { get; set;} = false;
        public bool IsMember { get; set;} = false;
        public bool IsWaitingApproval { get; set;} = false;
        public bool IsDeclined { get; set;} = false;
    }
}