import {AppUser} from "./AppUser";

export interface AuthUser {
    tokenString: string;
    appUser: AppUser;
}
