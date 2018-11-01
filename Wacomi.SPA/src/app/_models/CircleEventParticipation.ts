import { IAppUserLinkable } from "./IAppUserLinkable";

// public CircleEventForReturnDto CircleEvent { get; set;}
//         public int CircleEventId { get; set;} 
//         public CircleEventParticipationStatus Status { get; set;}
//         public string Message{get; set;}
//         public AppUserForReturnDto AppUser {get; set;}
//         public int? AppUserId {get; set;}
//         public DateTime DateCreated {get; set;}

export enum CircleEventParticipationStatus {
    Waiting = 1,
    Confirmed,
    Canceled
}

export interface CircleEventParticipation extends IAppUserLinkable {
    circleEventId: number;
    status: CircleEventParticipationStatus;
    message: string;
    dateCreated?: Date;
}
