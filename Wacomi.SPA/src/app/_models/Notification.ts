import { Photo } from "./Photo";

export interface AppNotification {
    id:number;
    notificationType: number;
    appUserId?: number;
    recordType? :string;
    recordId?: number;
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