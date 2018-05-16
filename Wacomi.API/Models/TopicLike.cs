using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class TopicLike
    {
        public int SupportMemberId{ get; set;}
        public int DairyTopicId { get; set;}
        public Member SupportMember{ get; set;}
        public DailyTopic DairyTopic { get; set;}
    }
}