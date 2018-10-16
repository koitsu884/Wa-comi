using System;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleEventParticipationForReturnDto
    {
        public CircleEventForReturnDto CircleEvent { get; set;}
        public int CircleEventId { get; set;} 
        public CircleEventParticipationStatus Status { get; set;}
        public string Message{get; set;}
        public AppUserForReturnDto AppUser {get; set;}
        public int? AppUserId {get; set;}
        public DateTime DateCreated {get; set;}
    }
}