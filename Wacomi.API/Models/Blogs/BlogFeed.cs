using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wacomi.API.Models
{
    public class BlogFeed
    {
        public int Id{ get; set;}
        public int BlogId { get; set;}
        public Blog Blog{get; set;}
        public DateTime? PublishingDate { get; set;} = DateTime.Now;
        public string Title{ get; set;}
        public string Url{ get; set;}
        public Photo Photo{get; set;}
        public int? PhotoId{get; set;}
        public bool IsActive{ get; set;} = true;
         public virtual ICollection<BlogFeedLike> FeedLikes { get; set;}
         public virtual ICollection<BlogFeedComment> FeedComments { get; set;}

         [NotMapped]
        public string ImageUrl{ get; set;}
         [NotMapped]
         public bool IsLiked{ get; set;} = false;
    }
}