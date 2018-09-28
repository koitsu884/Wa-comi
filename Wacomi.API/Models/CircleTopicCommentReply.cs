using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopicCommentReply : IAppUserLinkable, IStructuredDataItem
    {
        public int Id{ get; set;}
        public CircleTopicComment Comment{get; set;}
        [Required]
        public int CommentId{ get; set;}
        [Required]
        [MaxLength(1000)]
        public string Reply{ get; set;}
        public AppUser AppUser {get; set;}
        public int? AppUserId {get; set;}
        public bool IsActive {get; set;} = true;
        public int CircleId { get; set;}
        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}