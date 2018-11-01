import { IDataItemWithMultiplePhotos } from "./DataItemWithMultiplePhotos";
import { IAppUserLinkable } from "./IAppUserLinkable";
import { CircleEventParticipationStatus } from "./CircleEventParticipation";
import { Circle } from "./Circle";

// public int Id { get; set;}
//         public int CircleId{ get; set;}
//         public string Title { get; set;}
//         public string Description { get; set;}
//         public bool IsPublic{ get; set;} = false;
//         public string WebSiteUrls { get; set;}
//         public int? MaxNumber{ get; set;}
//         public bool ApprovalRequired{ get; set;} = false;
//         public string CityName { get; set;}
//         public int CityId{ get; set;}
//         public string Address{ get; set;}

//         public ICollection<PhotoForReturnDto> Photos {get; set;}
//         public PhotoForReturnDto MainPhoto {get; set;}
//         public int? MainPhotoId {get; set;}
//         public bool IsActive {get; set;}
//         public DateTime DateCreated {get; set;}
//         public DateTime DateUpdated {get; set;}
//         public AppUserForReturnDto AppUser {get; set;}
//         public int? AppUserId {get; set;}
//         public int NumberOfPaticipants{ get; set;}

export interface CircleEvent extends IDataItemWithMultiplePhotos, IAppUserLinkable {
    id: number;
    circleId: number;
    circle: Circle;
    title:string;
    description:string;
    isPublic:boolean;
    fromDate:string;
    toDate?:string;
    webSiteUrls:string;
    maxNumber?:number;
    approvalRequired: boolean;
    cityName: string;
    cityId: number;
    address: string;
    isActive:boolean;
    dateCreated:Date;
    dateUpdated:Date;
    numberOfPaticipants: number;
    numberOfWaiting: number;
    numberOfCanceled: number;
    
    myStatus?: CircleEventParticipationStatus;
    isCircleMember: boolean;
}
