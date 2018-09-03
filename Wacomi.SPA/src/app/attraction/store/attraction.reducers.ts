import * as fromApp from '../../store/app.reducer';
import * as AttractionActions from './attraction.actions';
import { Attraction } from '../../_models/Attraction';
import { Pagination } from '../../_models/Pagination';

export interface FeatureState extends fromApp.AppState {
    attraction: State
}

export interface State {
    selectedAttraction: Attraction;
    attractions: Attraction[];
    selectedCityId: number;
    selectedCategories: number[];
    pagination: Pagination,
    loading: boolean;
}

const initialState: State = {
    selectedAttraction: null,
    attractions: [],
    selectedCityId: null,
    selectedCategories: [],
    pagination: null,
    loading: false
};

export function attractionReducer(state = initialState, action: AttractionActions.AttractionActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case AttractionActions.SET_ATTRACTION:
            return {
                ...state,
                selectedAttraction: action.payload,
            }
        case AttractionActions.SET_ATTRACTION_CITY:
            return {
                ...state,
                selectedCityId: action.payload
            }
        case AttractionActions.SET_ATTRACTION_SEARCH_CATEGORIES:
            tempPagination = state.pagination;
            if (tempPagination)
                tempPagination.currentPage = 1;
            return {
                ...state,
                selectedCategories: action.payload,
               // pagination: tempPagination
            }
        case AttractionActions.CLEAR_ATTRACTION_FILTER:
            return {
                ...state,
                selectedCategories: [],
                selectedCityId: 0
            }
        // case ClanSeekActions.SET_CLANSEEK_PAGE:
        //     tempPagination = state.pagination;
        //     tempPagination.currentPage = action.payload;
        //     return {
        //         ...state,
        //         pagination: tempPagination
        //     }
        case AttractionActions.SEARCH_ATTRACTION:
            return {
                ...state,
                attractions: null,
                loading: true
            }
        case AttractionActions.SET_ATTRACTION_SEARCH_RESULT:
            return {
                ...state,
                attractions: action.payload.attractions,
                pagination: action.payload.pagination,
                loading: false,
                selectedAttraction: null
            }
        default:
            return state;
    }
}