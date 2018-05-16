import { Photo } from "./Photo";
import { Blog } from "./Blog";

export interface Member {
    id: number;
    username: string;
    displayName: string;
    dateOfBirth?: Date;
    gender: string;
    cityId?: number;
    city?: string;
    homeTownId?: number;
    homeTown?: string;
    mainPhotoUrl: string;
    introduction: string;
    interests: string;
    lastActive: Date;
    created: Date;
}
