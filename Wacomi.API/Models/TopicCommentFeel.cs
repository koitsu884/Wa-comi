using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public enum FeelingEnum {
        Like,
        Dislike,
        Hate,
    };
    public class TopicCommentFeel
    {
        public int? MemberId { get; set;}
        [Required]
        public int CommentId{ get; set;}
        public Member Member{ get; set;}
        public TopicComment Comment{ get; set;}
        public FeelingEnum Feeling{ get; set;}
    }
}