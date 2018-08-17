using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class BlogFeedComment
    {
        public int Id {get; set;}
        public int? BlogFeedId{get; set;}
        public BlogFeed BlogFeed{get; set;}

        public int? AppUserId { get; set;}
        public AppUser AppUser{ get; set;}
        public string DisplayName{ get; set;}
        public Photo Photo{ get; set;}
        public int? PhotoId { get; set;}
        [Required]
        [MaxLength(1000)]
        public string Comment{ get; set;}
        public System.DateTime DateCreated { get; set;} = System.DateTime.Now;
    }
}