using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicReply
    {
        public int Id {get; set;}
        [Required]
        public int TopicCommentId{get; set;}
        public TopicComment TopicComment{get; set;}

        public int? MemberId { get; set;}
        public MemberProfile Member{ get; set;}
        public string DisplayName{ get; set;}
        public string MainPhotoUrl{ get; set;}
        [Required]
        [MaxLength(1000)]
        public string Reply{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;

    }
}