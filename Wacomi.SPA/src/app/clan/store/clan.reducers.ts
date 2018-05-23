import * as ClanSeekActions from './clan.actions';
import { ClanSeek } from '../../_models/ClanSeek';
import * as fromApp from '../../store/app.reducer';

export interface FeatureState extends fromApp.AppState {
    clan : State
}

export interface State {
    clan: ClanSeek;
}

const initialState: State = {
    clan: null
};

export function clanSeekReducer(state = initialState, action: ClanSeekActions.ClanSeekActions ){
    switch(action.type){
        case ClanSeekActions.SET_CLANSEEK:
            return {
                ...state,
                clan: action.payload,
            }
        default:
            return state;
    }
}