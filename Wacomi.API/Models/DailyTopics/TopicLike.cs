using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicLike
    {
        public int SupportAppUserId{ get; set;}
        public int DailyTopicId { get; set;}
        public AppUser SupportAppUser{ get; set;}
        public DailyTopic DailyTopic { get; set;}
    }
}