using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public enum FeelingEnum {
        Like = 1,
        Dislike,
        Hate,
    };
    public class TopicCommentFeel
    {
        [Required]
        public int AppUserId { get; set;}
        [Required]
        public int CommentId{ get; set;}
        public AppUser AppUser{ get; set;}
        public TopicComment Comment{ get; set;}
        [Required]
        public FeelingEnum Feeling{ get; set;}
    }
}