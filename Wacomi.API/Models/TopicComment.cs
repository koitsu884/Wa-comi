using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicComment
    {
        public int Id{ get; set;}
        public Member Member{ get; set;}
        public int? MemberId { get; set;}
        [Required]
        public string Comment{ get; set;}
        public ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
    }
}