import * as AccountActions from './account.actions';
import { AppUser } from '../../_models/AppUser';
import * as fromApp from '../../store/app.reducer';
import { MemberProfile } from '../../_models/MemberProfile';
import { BusinessProfile } from '../../_models/BusinessProfile';
import { UserAccount } from '../../_models/UserAccount';

export interface State {
    token: string;
    authenticated: boolean;
    account: UserAccount;
    appUser: AppUser;
    memberProfile: MemberProfile;
    businessProfile: BusinessProfile;
    isAdmin: boolean;
}

const initialState: State = {
    authenticated: false,
    token: null,
    account: null,
    appUser: null,
    memberProfile: null,
    businessProfile: null,
    isAdmin: false
};

export function accountReducer(state = initialState, action: AccountActions.AccountActions ){
    switch(action.type){
        case AccountActions.LOGOUT:
        case AccountActions.TOKEN_EXPIRED:
            localStorage.removeItem('token');
            localStorage.removeItem('appUser');
            localStorage.removeItem('account');
            localStorage.removeItem('memberProfile');
            localStorage.removeItem('businessProfile');
            localStorage.removeItem('isAdmin');
            localStorage.removeItem('photos');
            return {
                ...state,
                authenticated: false,
                appUser: null,
                token: null,
                memberProfile: null,
                businessProfile: null,
                isAdmin: false,
            };
        case AccountActions.SET_TOKEN:
            localStorage.setItem('token', action.payload);
            return {
                ...state,
                    token: action.payload,
                    authenticated: true
                };
        case AccountActions.SET_APPUSER:
            var appUser = action.payload;
            localStorage.setItem('appUser', JSON.stringify(action.payload));
            return {
                ...state,
                appUser: action.payload
            };
            case AccountActions.SET_ACCOUNT:
            localStorage.setItem('account', JSON.stringify(action.payload));
            return {
                ...state,
                account: action.payload
            };
        case AccountActions.SET_MEMBER_PROFILE:
            if(action.payload == null)
                return state;
            localStorage.setItem('memberProfile', JSON.stringify(action.payload));
            localStorage.removeItem('businessProfile');
            return {
                ...state,
                memberProfile: action.payload,
                businessProfile: null
            };
        case AccountActions.SET_BUSINESS_PROFILE:
            if(action.payload == null)
                return state;
            localStorage.setItem('businessProfile', JSON.stringify(action.payload));
            localStorage.removeItem('memberProfile');
            return {
                ...state,
                businessProfile: action.payload,
                memberProfile: null
            };
        case AccountActions.SET_ADMIN_FLAG:
        localStorage.setItem('isAdmin', JSON.stringify(action.payload));
        return {
            ...state,
            isAdmin: action.payload,
        };
        default:
            return state;
    }
}