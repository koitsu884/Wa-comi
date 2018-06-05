using System;

namespace Wacomi.API.Dto
{
    public class TopicReplyForReturnDto
    {
        public int Id{ get; set;}
        public int TopicCommentId { get; set;}
        public int? MemberId { get; set;}
        public string DisplayName{ get; set;}
        public string MainPhotoUrl{ get; set;}
        public string Reply{ get; set;}
        public DateTime Created{ get; set;}
    }
}