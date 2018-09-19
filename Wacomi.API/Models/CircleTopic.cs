using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleTopic : UserRecord, ISinglePhotoAttachable
    {
        public Circle Circle{ get; set;}
        [Required]
        public int CircleId{ get; set;}
        [Required]
        [MaxLength(100)]
        public string Title{ get; set;}
        [Required]
        [MaxLength(5000)]
        public string Description{ get; set;}
        public Photo Photo { get; set;}
        public int PhotoId {get; set;}
        public virtual ICollection<CircleTopicComment> TopicComments{ get; set;} 
    }
}