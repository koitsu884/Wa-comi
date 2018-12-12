import { Photo } from "./Photo";
import { IDataItem } from "./IDataItem";
import { IAppUserLinkable } from "./IAppUserLinkable";
import { ShortComment } from "./ShortComment";

// public int Id { get; set;}
//         public int CircleTopicId{ get; set;}
//         public string Comment{ get; set;}
//         public PhotoForReturnDto Photo { get;set;}
//         public int? PhotoId {get; set;}
//         public int ReplyCount{ get; set;}
//         public AppUserForReturnDto AppUser {get;set;}
//         public int? AppUserId {get;set;}
//         public DateTime DateCreated {get; set;}
//         public DateTime DateUpdated {get; set;}
        
export interface CircleTopicComment extends IAppUserLinkable, IDataItem {
    circleTopicId: number;
    comment:string;
    photo: Photo;
    photoId: number;
    replyCount:number;

    //Client side only
    displayReplies?: boolean;
    //topicReplies: TopicReply[];
    topicReplies?: ShortComment[];
}
