using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wacomi.API.Models
{
    public enum NotificationEnum {
        NewMessage = 1,
        //Blog feed notifications
        NewPostOnFeedComment,
        RepliedOnFeedComment,
        //Daily topic notifications
        NewPostOnTopicComment,
        RepliedOnTopicComment,
        //Circle notifications
        NewCircleMemberRequest,
        CircleRequestAccepted,
        // NewCircleMemberJoined, //多分くどいので要らない
        NewCircleTopicCreated,
        NewCircleCommentReplyByMember,
        NewCircleCommentReplyByOwner,
        // Circle Event Notifications
        NewCircleEventParticipationRequest,
        EventParticipationRequestAccepted
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
        private string _relatingRecordIds;
        [NotMapped]
        public JObject RelatingRecordIds
        {
            get
            {
                return JsonConvert.DeserializeObject<JObject>(string.IsNullOrEmpty(_relatingRecordIds) ? "{}" : _relatingRecordIds);
            }
            set {
                _relatingRecordIds = value.ToString();
            }
        }
        public virtual Photo Photo { get; set;}
        public int? PhotoId { get; set;}
        public string TargetRecordTitle{ get; set;}
        public string FromUserName{ get; set;}
        public string Message {get; set;}
    }
}