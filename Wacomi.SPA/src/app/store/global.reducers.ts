import { Hometown } from "../_models/Hometown";
import { City } from "../_models/City";
import * as GlobalActions from './global.actions';
import { KeyValue } from "../_models/KeyValue";

export interface State {
    cityList: City[];
    homeTownList: KeyValue[];
    clanSeekCategories: KeyValue[];
}

const initialState: State = {
    cityList: null,
    homeTownList: null,
    clanSeekCategories : null
};

export function globalReducer(state = initialState, action: GlobalActions.GlobalActions ){
    switch(action.type){
        case GlobalActions.SET_CITY_LIST:
            return {
                ...state,
                cityList: action.payload
            }
        case GlobalActions.SET_HOMETOWN_LIST:
        return {
            ...state,
            homeTownList: action.payload
        }
        case GlobalActions.SET_CLANCATEGORY_LIST:
        return {
            ...state,
            clanSeekCategories: action.payload
        }
        default:
            return state;
    }
}