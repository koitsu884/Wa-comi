using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public enum CircleEventParticipationStatus {
        Waiting,
        Confirmed,
        Canceled
    }
    public class CircleEventParticipation : IAppUserLinkable
    {
        public CircleEvent CircleEvent { get; set;}
        [Required]
        public int CircleEventId { get; set;} 
        public CircleEventParticipationStatus Status { get; set;} = CircleEventParticipationStatus.Waiting;
        [MaxLength(StaticData.MidTextLength)]
        public string Message{get; set;}
        public AppUser AppUser {get; set;}
        [Required]
        public int? AppUserId {get; set;}
        public DateTime DateCreated {get; set;} = DateTime.Now;
    }
}