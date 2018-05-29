using System;
using System.Collections.Generic;

namespace Wacomi.API.Models
{
    public class DailyTopic
    {
        public int Id {get; set;}
        public bool IsTemporary{get; set;} = true;
        public bool IsActive{ get; set;} = false;
        public virtual ICollection<TopicLike> TopicLikes{ get; set;}
        public string UserId{ get; set;} //to limit post count
        public string Title{get; set;}
        public DateTime Created{ get; set;} = DateTime.Now;
        public DateTime LastDiscussed{ get; set;} = DateTime.Now;        
    }
}