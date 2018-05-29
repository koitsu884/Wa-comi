using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicLike
    {
        public string SupportUserId{ get; set;}
        public int DailyTopicId { get; set;}
        public AppUser SupportUser{ get; set;}
        public DailyTopic DailyTopic { get; set;}
    }
}