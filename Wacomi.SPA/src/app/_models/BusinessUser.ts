import { Photo } from "./Photo";
import { Blog } from "./Blog";

export interface BusinessUser {
    id: number;
    username: string;
    displayName: string;
    establishedDate?: Date;
    city?: string;
    cityId?: number;
    mainPhotoUrl: string;
    introduction: string;
    lastActive: Date;
    created: Date;
}
