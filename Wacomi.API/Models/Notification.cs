using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public enum NotificationEnum {
        NewMessage = 1,
        NewPostOnFeedComment,
        RepliedOnFeedComment,
        NewPostOnTopicComment,
        RepliedOnTopicComment,
    }
    public class Notification
    {
        public int Id { get; set;}
        [Required]
        public int AppUserId { get; set;}
        [Required]
        public NotificationEnum NotificationType { get; set;}
        public string RecordType { get; set;}
        public int RecordId { get; set;}
        public virtual Photo Photo { get; set;}
        public int? PhotoId { get; set;}
        public string Message {get; set;}
    }
}