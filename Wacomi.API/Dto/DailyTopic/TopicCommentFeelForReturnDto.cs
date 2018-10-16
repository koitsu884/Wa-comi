using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class TopicCommentFeelForReturnDto
    {
        public int MemberId { get; set;}
        public int CommentId{ get; set;}
        public FeelingEnum Feeling{ get; set;}
    }
}