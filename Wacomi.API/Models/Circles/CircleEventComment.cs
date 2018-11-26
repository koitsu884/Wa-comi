using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models.Circles
{
    public class CircleEventComment : IAppUserLinkable, IStructuredDataItem
    {
        public CircleEvent CircleEvent { get; set;}
        [Required]
        public int CircleEventId{ get; set;}
        public int CircleId{ get; set;}
        [Required]
        [MaxLength(2000)]
        public string Comment{ get; set;}
        public virtual ICollection<CircleEventCommentReply> Replies{ get; set;}
        public AppUser AppUser {get;set;}
        public int? AppUserId {get;set;}
        public bool IsActive {get; set;} = true;
        public int Id {get; set;}
        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}