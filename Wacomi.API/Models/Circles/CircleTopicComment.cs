using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopicComment : IAppUserLinkable, IDataItemWithSinglePhoto
    {
        public CircleTopic CircleTopic { get; set;}
        [Required]
        public int CircleTopicId{ get; set;}
        public int CircleId{ get; set;}
        [Required]
        [MaxLength(2000)]
        public string Comment{ get; set;}
        public Photo Photo { get;set;}
        public int? PhotoId {get; set;}
        public virtual ICollection<CircleTopicCommentReply> Replies{ get; set;}
        public AppUser AppUser {get;set;}
        public int? AppUserId {get;set;}
        public bool IsActive {get; set;} = true;
        public int Id {get; set;}
        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}