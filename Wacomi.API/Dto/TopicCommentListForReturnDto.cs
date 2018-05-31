using System;

namespace Wacomi.API.Dto
{
    public class TopicCommentListForReturnDto
    {
        public string MainPhotoUrl{ get; set;}
        public string DisplayName{ get; set;}
        public string Comment{ get; set;}
        public int LikedCount{ get; set;}
        public int ReplyCount{ get; set;}
        public DateTime Created{ get; set;}
    }
}