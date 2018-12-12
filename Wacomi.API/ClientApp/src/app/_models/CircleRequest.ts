import { AppUser } from "./AppUser";
import { Circle } from "./Circle";

export interface CircleRequest {
    appUser: AppUser;
    appUserId: number;
    circle: Circle;
    circleId: number;
    declined: boolean;
    message: string;
}

// public AppUser AppUser{ get; set;}
//         [Required]
//         public int AppUserId { get; set; }
//         public Circle Circle {get; set;}
//         [Required]
//         public int CircleId{ get; set;}
//         bool Declined{ get; set;} = false;
//         [MaxLength(1000)]
//         string Message { get; set;}