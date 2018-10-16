using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class BlogFeedLike
    {
        public int Id{ get; set;}
        public int? SupportAppUserId{ get; set;}
        public int? BlogFeedId { get; set;}
        public AppUser SupportAppUser{ get; set;}
        public BlogFeed BlogFeed { get; set;}
    }
}