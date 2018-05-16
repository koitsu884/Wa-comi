using System;
using System.Collections.Generic;

namespace Wacomi.API.Models
{
    public class DailyTopic
    {
        public int Id {get; set;}
        public bool IsActive{get; set;}
        public ICollection<TopicLike> TopicLikes{ get; set;}
        public string Title{get; set;}
        public DateTime Created{ get; set;}
        public DateTime LastDiscussed{ get; set;}        
    }
}