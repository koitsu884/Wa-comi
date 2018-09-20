import * as fromApp from '../../store/app.reducer';
import * as CircleActions from './circle.actions';
import { Pagination } from '../../_models/Pagination';
import { Circle } from '../../_models/Circle';
import { CircleSearchOptions } from '../../_models/CircleSearchOptions';

export interface FeatureState extends fromApp.AppState {
    circle: State
}

export interface State {
    selectedCircle: Circle;
    circles: Circle[];
    searchParam: CircleSearchOptions;
    pagination: Pagination
}

const initialState: State = {
    selectedCircle: null,
    circles: [],
    searchParam: <CircleSearchOptions>{
        categoryId: 0,
        cityId: 0,
    },
    pagination: null
};

export function circleReducer(state = initialState, action: CircleActions.CircleActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case CircleActions.GET_CIRCLE:
            return {
                ...state,
                selectedCircle: null,
            }
        case CircleActions.SET_CIRCLE:
            return {
                ...state,
                selectedCircle: action.payload,
            }
        case CircleActions.SET_CIRCLE_SEARCH_OPTIONS:
            return {
                ...state,
                searchParam: action.payload,
            }
        case CircleActions.SET_CIRCLE_PAGE:
            return {
                ...state,
                pagination: action.payload,
            }
        case CircleActions.SEARCH_CIRCLE:
            return {
                ...state,
                circles: null,
                pagination: null
            }
        case CircleActions.SET_CIRCLE_SEARCH_RESULT:
            return {
                ...state,
                circles: action.payload.circles,
                pagination: action.payload.pagination,
                selectedCircle: null
            }
        default:
            return state;
    }
}