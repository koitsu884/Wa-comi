import { Hometown } from "../_models/Hometown";
import { City } from "../_models/City";
import * as GlobalActions from './global.actions';
import { KeyValue } from "../_models/KeyValue";
import { Category } from "../_models/Category";

export interface State {
    cityList: City[];
    homeTownList: KeyValue[];
    clanSeekCategories: KeyValue[];
    attractionCategories: Category[];
}

const initialState: State = {
    cityList: null,
    homeTownList: null,
    clanSeekCategories : null,
    attractionCategories: null
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
        var tempList: any = action.payload;
        tempList.push({id:null, name:"その他"});
        return {
            ...state,
            clanSeekCategories: action.payload
        }
        case GlobalActions.SET_ATTRACTION_CATEGORY_LIST:
        return {
            ...state,
            attractionCategories: action.payload
        }
        default:
            return state;
    }
}