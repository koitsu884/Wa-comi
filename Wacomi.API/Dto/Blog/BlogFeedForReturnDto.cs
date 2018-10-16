using System;

namespace Wacomi.API.Dto
{
    public class BlogFeedForReturnDto
    {
        public int Id{ get; set;}
        public int BlogId { get; set;}
        public string BlogTitle { get; set;}
        public DateTime? PublishingDate { get; set;}
        public string Title{ get; set;}
        public string Url{ get; set;}
        public string BlogImageUrl{ get; set;}
        public PhotoForReturnDto Photo{ get; set;}
        public string WriterName{ get; set;}
        public int OwnerId{ get; set;}
        public int LikedCount{ get; set;}
        public int CommentCount{ get; set;}

        public bool IsLiked{ get; set;}

    }
}