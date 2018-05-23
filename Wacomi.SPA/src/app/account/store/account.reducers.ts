import * as AccountActions from './account.actions';
import { AppUser } from '../../_models/AppUser';
import * as fromApp from '../../store/app.reducer';
import { Member } from '../../_models/Member';
import { BusinessUser } from '../../_models/BusinessUser';

export interface State {
    token: string;
    authenticated: boolean;
    appUser: AppUser;
    memberProfile: Member;
    bisuserProfile: BusinessUser;
}

const initialState: State = {
    authenticated: false,
    token: null,
    appUser: null,
    memberProfile: null,
    bisuserProfile: null
};

export function accountReducer(state = initialState, action: AccountActions.AccountActions ){
    switch(action.type){
        case AccountActions.LOGOUT:
            localStorage.removeItem('token');
            localStorage.removeItem('appUser');
            localStorage.removeItem('profile');
            localStorage.removeItem('photos');
            return {
                ...state,
                authenticated: false,
                appUser: null,
                token: null,
                memberProfile: null,
                bisuserProfile: null
            };
        case AccountActions.SET_TOKEN:
            localStorage.setItem('token', action.payload.token);
        return {
             ...state,
                token: action.payload.token,
                authenticated: true
            };
        case AccountActions.SET_APPUSER:
            localStorage.setItem('appUser', JSON.stringify(action.payload));
            return {
                ...state,
                appUser: action.payload
            };
        case AccountActions.SET_MEMBER:
            localStorage.setItem('profile', JSON.stringify(action.payload));
            return {
                ...state,
                memberProfile: action.payload,
                bisuserProfile: null
            };
        case AccountActions.SET_BISUSER:
            localStorage.setItem('profile', JSON.stringify(action.payload));
            return {
                ...state,
                bisuserProfile: action.payload,
                memberProfile: null
            };
        default:
            return state;
    }
}