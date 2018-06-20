import {AppUser} from "./AppUser";
import { MemberProfile } from "./MemberProfile";
import { BusinessProfile } from "./BusinessProfile";
import { Photo } from "./Photo";
import { Blog } from "./Blog";

export interface LoginResult {
    tokenString: string;
    // userName: string;
    // password: string;
    appUser: AppUser;
    account: Account;
    photos: Photo[];
    blogs: Blog[];
    memberProfile: MemberProfile;
    businessProfile: BusinessProfile;
    isAdmin: boolean;
}
