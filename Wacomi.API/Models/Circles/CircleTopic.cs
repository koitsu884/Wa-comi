using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopic : IDataItemWithSinglePhoto, IAppUserLinkable
    {
        public int Id {get; set;}
        public Circle Circle{get; set;}
        [Required]
        public int CircleId{ get; set;}
        public virtual ICollection<CircleTopicComment> TopicComments {get; set;}
        public bool IsSecret {get; set;} = false;
        [Required]
        [MaxLength(100)]
        public string Title{get; set;}
        [Required]
        [MaxLength(5000)]
        public string Description{ get; set;}
        public bool IsActive {get; set;} = true;
        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
        public AppUser AppUser {get; set;}
        public int? AppUserId {get; set;}
        public Photo Photo {get; set;}
        public int? PhotoId {get; set;}
    }
}