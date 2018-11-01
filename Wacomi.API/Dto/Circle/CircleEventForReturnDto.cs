using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleEventForReturnDto
    {
        public int Id { get; set;}
        public int CircleId{ get; set;}
        public CircleForReturnDto Circle{ get; set;}
        public string Title { get; set;}
        public string Description { get; set;}
        public bool IsPublic{ get; set;} = false;
        public DateTime FromDate { get; set;}
        public DateTime? ToDate{ get; set;}
        public string WebSiteUrls { get; set;}
        public int? MaxNumber{ get; set;}
        public bool ApprovalRequired{ get; set;} = false;
        public string CityName { get; set;}
        public int CityId{ get; set;}
        public string Address{ get; set;}

        public ICollection<PhotoForReturnDto> Photos {get; set;}
        public PhotoForReturnDto MainPhoto {get; set;}
        public int? MainPhotoId {get; set;}
        public bool IsActive {get; set;}
        public DateTime DateCreated {get; set;}
        public DateTime DateUpdated {get; set;}
        public AppUserForReturnDto AppUser {get; set;}
        public int? AppUserId {get; set;}
        public int NumberOfPaticipants{ get; set;}
        public int NumberOfWaiting{ get; set;}
        public int NumberOfCanceled{ get; set;}
        public CircleEventParticipationStatus? MyStatus { get; set;}
        public Boolean IsCircleMember{ get; set;} = false;
    }
}