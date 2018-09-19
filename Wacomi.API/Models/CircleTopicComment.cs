using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopicComment : UserRecord, ISinglePhotoAttachable
    {
        public CircleTopic CircleTopic { get; set;}
        [Required]
        public int CircleTopicId{ get; set;}
        [Required]
        [MaxLength(2000)]
        public string Comment{ get; set;}
        public Photo Photo { get;set;}
        public int PhotoId {get; set;}
        public virtual ICollection<CircleTopicCommentReply> Replies{ get; set;}
    }
}