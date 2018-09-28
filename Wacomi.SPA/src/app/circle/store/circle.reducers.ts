import * as fromApp from '../../store/app.reducer';
import * as CircleActions from './circle.actions';
import * as fromCircleTopic from './circletopic.reducers';
import * as fromCircleMember from './circlemember.reducers';
import { Pagination } from '../../_models/Pagination';
import { Circle } from '../../_models/Circle';
import { CircleSearchOptions } from '../../_models/CircleSearchOptions';
import { CircleMember } from '../../_models/CircleMember';
import { ActionReducerMap } from '@ngrx/store';
import { CircleRequest } from '../../_models/CircleRequest';
import { CircleTopic } from '../../_models/CircleTopic';

export interface FeatureState extends fromApp.AppState {
    circleModule: CircleState
}

export interface CircleState {
    circle: State;
    circleTopic: fromCircleTopic.State;
    circleMember: fromCircleMember.State;
}

export const reducers: ActionReducerMap<CircleState> = {
    circle: circleReducer,
    circleTopic: fromCircleTopic.circleTopicReducer,
    circleMember: fromCircleMember.circleMemberReducer,
};

export interface State {
    selectedCircle: Circle;
    latestMemberList: CircleMember[];
    latestTopicList: CircleTopic[];
    circleRequests: CircleRequest[];
    circles: Circle[];
    searchParam: CircleSearchOptions;
    pagination: Pagination
}

const initialState: State = {
    selectedCircle: null,
    latestMemberList: [],
    latestTopicList: [],
    circleRequests: [],
    circles: [],
    searchParam: <CircleSearchOptions>{
        categoryId: null,
        cityId: null,
    },
    pagination: null
};

export function circleReducer(state = initialState, action: CircleActions.CircleActions) {
    let tempPagination: Pagination;
    let tempRequest: CircleRequest[];
    let index: number;
    switch (action.type) {
        case CircleActions.INIT_CIRCLE_STATE:
            return {
                ...state,
                selectedCircle: null,
                circles: [],
                searchParam: <CircleSearchOptions>{
                    categoryId: null,
                    cityId: null,
                },
                pagination: null
            }
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
        case CircleActions.GET_LATEST_CIRCLE_TOPIC_LIST:
            return {
                ...state,
                latestTopicList: [],
            }

        case CircleActions.SET_LATEST_CIRCLE_TOPIC_LIST:
            return {
                ...state,
                latestTopicList: action.payload,
            }
        case CircleActions.ADD_NEW_CIRCLE_TOPIC:
            return {
                ...state,
                latestTopicList: [...state.latestTopicList, action.payload],
            }
        case CircleActions.GET_LATEST_CIRCLE_MEMBER_LIST:
            return {
                ...state,
                latestMemberList: [],
            }
        case CircleActions.SET_LATEST_CIRCLE_MEMBER_LIST:
            return {
                ...state,
                latestMemberList: action.payload,
            }
        case CircleActions.ADD_NEW_CIRCLE_MEMBER:
            return {
                ...state,
                latestMemberList: [...state.latestMemberList, action.payload],
            }
        case CircleActions.SET_CIRCLE_SEARCH_OPTIONS:
            return {
                ...state,
                searchParam: action.payload,
            }
        case CircleActions.SET_CIRCLE_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        case CircleActions.SEARCH_CIRCLE:
            return {
                ...state,
                circles: null
            }
        case CircleActions.SET_CIRCLE_SEARCH_RESULT:
            return {
                ...state,
                circles: action.payload.circles,
                pagination: action.payload.pagination,
                selectedCircle: null
            }
        case CircleActions.GET_CIRCLE_REQUEST_LIST:
            return {
                ...state,
                circleRequests: []
            }
        case CircleActions.SET_CIRCLE_REQUEST_LIST:
            return {
                ...state,
                circleRequests: action.payload
            }
        case CircleActions.APPROVE_CIRCLE_REQUEST:
            tempRequest = [...state.circleRequests];
            index = tempRequest.findIndex(x => x == action.payload);
            tempRequest.splice(index, 1);
            return {
                ...state,
                circleRequests: tempRequest
            }
        case CircleActions.DECLINE_CIRCLE_REQUEST:
            tempRequest = [...state.circleRequests];
            index = tempRequest.findIndex(x => x == action.payload);
            tempRequest[index].declined = true;
            return {
                ...state,
                circleRequests: tempRequest
            }
        default:
            return state;
    }
}