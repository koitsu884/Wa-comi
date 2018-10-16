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

        public int? AppUserId { get; set;}
        public AppUser AppUser{ get; set;}
        public string DisplayName{ get; set;}
        public Photo Photo{ get; set;}
        public int? PhotoId { get; set;}
        [Required]
        [MaxLength(1000)]
        public string Reply{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;

    }
}