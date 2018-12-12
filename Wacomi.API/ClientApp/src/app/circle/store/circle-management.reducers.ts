import * as CircleManagementActions from '../store/circle-management.actions';
import { Circle } from '../../_models/Circle';
import { Pagination } from '../../_models/Pagination';

export interface State {
    myCircleList: Circle[];
    myCirclePagination: Pagination,
    ownCircleList: Circle[];
    ownCirclePagination: Pagination
}

const initialState: State = {
    myCircleList: [],
    myCirclePagination: null,
    ownCircleList: [],
    ownCirclePagination: null
};

export function circleManagementReducer(state = initialState, action: CircleManagementActions.CircleManagementActions) {
    switch (action.type) {
        case CircleManagementActions.GET_MY_CIRCLE_LIST:
            return {
                ...state,
                myCircleList: [],
            }
        case CircleManagementActions.SET_MY_CIRCLE_LIST:
            return {
                ...state,
                myCircleList: action.payload.circles,
                myCirclePagination: action.payload.pagination
            }
        case CircleManagementActions.GET_OWN_CIRCLE_LIST:
            return {
                ...state,
                ownCircleList: [],
            }
        case CircleManagementActions.SET_OWN_CIRCLE_LIST:
            return {
                ...state,
                ownCircleList: action.payload.circles,
                ownCirclePagination: action.payload.pagination
            }
        default:
            return state;
    }
}