import { Photo } from "./Photo";
import { IDataItem } from "./IDataItem";
import { IAppUserLinkable } from "./IAppUserLinkable";

export interface CircleTopic  extends IAppUserLinkable, IDataItem{
    circleId: number;
    topicCommentCounts?: number;
    isSecret?: boolean;
    title:string;
    description:string;
    isActive?:boolean;
    photo: Photo;
    photoId?: number;
}

// public int Id {get; set;}
//         public CircleForReturnDto Circle{get; set;}
//         public int CircleId{ get; set;}
//         public int TopicCommentCounts {get; set;}
//         public bool IsSecret {get; set;}
//         public string Title{get; set;}
//         public string Description{ get; set;}
//         public bool IsActive {get; set;}
//         public DateTime DateCreated {get; set;}
//         public DateTime DateUpdated {get; set;}
//         public AppUserForReturnDto AppUser {get; set;}
//         public int? AppUserId {get; set;}
//         public PhotoForReturnDto Photo {get; set;}
//         public int? PhotoId {get; set;}