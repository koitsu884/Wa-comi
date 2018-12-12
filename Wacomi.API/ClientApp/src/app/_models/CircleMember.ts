import { Circle } from "./Circle";
import { AppUser } from "./AppUser";

export enum CircleRoleEnum {
    OWNER,
    MEMBER
}

export interface CircleMember {
    circle : Circle;
    circleId:number;
    appUser: AppUser;
    appUserId: number;
    role: CircleRoleEnum;
    dateJoined: Date;
    dateLastActive: Date;
}


// public Circle Circle{get; set;}
// public int CircleId{ get; set;}
// public AppUser AppUser { get; set;}
// public int AppUserId { get; set;}
// public CircleRoleEnum Role { get; set;}
// public int? ApprovedBy{ get; set;}