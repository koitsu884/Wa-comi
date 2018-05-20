using System;

namespace Wacomi.API.Models
{
    public class BlogFeed
    {
        public int Id{ get; set;}
        public int BlogId { get; set;}
        public Blog Blog{get; set;}
        public DateTime? PublishingDate { get; set;}
        public string Title{ get; set;}
        public string Url{ get; set;}
        public string ImageUrl{ get; set;}
    }
}