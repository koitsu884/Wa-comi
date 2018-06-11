using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicComment
    {
        public int Id{ get; set;}
        public int? MemberId { get; set;}
        public MemberProfile Member{ get; set;}
        public string DisplayName{ get; set;}
        public string MainPhotoUrl{ get; set;}
        public string TopicTitle{ get; set;}
        [Required]
        [MaxLength(100)]
        public string Comment{ get; set;}
        public ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
        public ICollection<TopicReply> TopicReplies { get; set;}
        public DateTime Created{ get; set;} = DateTime.Now;
    }
}