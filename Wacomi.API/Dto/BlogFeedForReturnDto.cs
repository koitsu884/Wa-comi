using System;

namespace Wacomi.API.Dto
{
    public class BlogFeedForReturnDto
    {
        public int Id{ get; set;}
        public int BlogId { get; set;}
        public DateTime? PublishingDate { get; set;}
        public string Title{ get; set;}
        public string Url{ get; set;}
        public string ImageUrl{ get; set;}
        public string BlogImageUrl{ get; set;}
        public string WriterName{ get; set;}
        public int OwnerId{ get; set;}

    }
}