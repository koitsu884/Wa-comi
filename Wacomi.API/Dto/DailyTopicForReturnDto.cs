using System;

namespace Wacomi.API.Dto
{
    public class DailyTopicForReturnDto
    {
        public int Id {get; set;}
        public bool IsTemporary{get; set;} = true;
        public bool IsActive{get; set;} = false;
        public bool IsLiked{ get; set;}
        public int LikedCount {get; set;}
        public string Title{get; set;}
        public DateTime Created{ get; set;} = DateTime.Now;
        public DateTime LastDiscussed{ get; set;} = DateTime.Now;   
    }
}