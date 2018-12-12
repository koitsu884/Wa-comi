import { AppUser } from "./AppUser";
import { MemberProfile } from "./MemberProfile";
import { BusinessProfile } from "./BusinessProfile";

export interface UserDetails {
    appUser: AppUser;
    memberProfile?: MemberProfile;
    businessProfile?: BusinessProfile;
}
