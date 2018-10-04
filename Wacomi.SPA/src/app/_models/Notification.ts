import { Photo } from "./Photo";

// public enum NotificationEnum {
//     NewMessage = 1,
//     //Blog feed notifications
//     NewPostOnFeedComment,
//     RepliedOnFeedComment,
//     //Daily topic notifications
//     NewPostOnTopicComment,
//     RepliedOnTopicComment,
//     //Circle notifications
//     NewCircleMemberRequest,
//     CircleRequestAccepted,
//     // NewCircleMemberJoined, //多分くどいので要らない
//     NewCircleTopicCreated,
//     NewCircleCommentReplyByMember,
//     NewCircleCommentReplyByOwner
// }

export enum NotificationEnum {
    NewMessage = 1,
    NewPostOnFeedComment,
    RepliedOnFeedComment,
    NewPostOnTopicComment,
    RepliedOnTopicComment,
    NewCircleMemberRequest,
    CircleRequestAccepted,
    NewCircleTopicCreated,
    NewCircleCommentReplyByMember,
    NewCircleCommentReplyByOwner
 };

export interface AppNotification {
    id:number;
    notificationType: number;
    appUserId?: number;
    recordType? :string;
    recordId?: number;
    relatingRecordIds?: any;
    photo: Photo;
    message:string;
}


// public class Notification
// {
//     public int Id { get; set;}
//     [Required]
//     public int AppUserId { get; set;}
//     [Required]
//     public NotificationEnum NotificationType { get; set;}
//     public string RecordType { get; set;}
//     public int RecordId { get; set;}
// }