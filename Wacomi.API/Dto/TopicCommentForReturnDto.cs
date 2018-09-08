using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class TopicCommentForReturnDto
    {
        public int Id{ get; set;}
        public int MemberId { get; set;}
        public int AppUserId { get; set;}
        public string DisplayName { get; set;}
        public string IconUrl { get; set;}
        public string TopicTitle{ get; set;}
        public string Comment{ get; set;}
        public virtual ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
        public virtual ICollection<TopicReply> TopicReplies { get; set;}
        public DateTime Created{ get; set;}
    }
}