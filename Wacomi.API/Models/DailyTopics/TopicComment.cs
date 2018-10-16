using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicComment
    {
        public int Id{ get; set;}
        public int? AppUserId { get; set;}
        public AppUser AppUser{ get; set;}
        public string DisplayName{ get; set;}
        public Photo Photo{ get; set;}
        public int? PhotoId{ get; set;}
        public string TopicTitle{ get; set;}
        [Required]
        [MaxLength(100)]
        public string Comment{ get; set;}
        public ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
        public ICollection<TopicReply> TopicReplies { get; set;}
        public DateTime Created{ get; set;} = DateTime.Now;
    }
}