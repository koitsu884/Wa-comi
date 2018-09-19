using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopicCommentReply : UserRecord
    {
        public CircleTopicComment Comment{get; set;}
        public int CommentId{ get; set;}
        [Required]
        [MaxLength(1000)]
        public string Reply{ get; set;}
    }
}