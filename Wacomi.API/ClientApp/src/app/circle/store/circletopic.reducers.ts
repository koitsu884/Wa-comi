import * as CircleTopicActions from '../store/circletopic.actions';
import { CircleTopic } from "../../_models/CircleTopic";
import { Pagination } from "../../_models/Pagination";

export interface State {
    latestTopicList: CircleTopic[];
    topicList: CircleTopic[];
    pagination: Pagination;
    selectedCircleTopic: CircleTopic;
}

const wtfInitialState: State = {
    latestTopicList: [],
    topicList: [],
    pagination: null,
    selectedCircleTopic: null,
};

export function circleTopicReducer(state = wtfInitialState, action: CircleTopicActions.CircleTopicActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case CircleTopicActions.GET_CIRCLE_TOPIC:
            return {
                ...state,
                selectedCircleTopic: null
            }
        case CircleTopicActions.SET_CIRCLE_TOPIC:
            return {
                ...state,
                selectedCircleTopic: action.payload
            }
        case CircleTopicActions.GET_CIRCLE_TOPIC_LIST:
            return {
                ...state,
                topicList: [],
            }

        case CircleTopicActions.SET_CIRCLE_TOPIC_LIST:
            return {
                ...state,
                topicList: action.payload.topicList,
                pagination: action.payload.pagination
            }
        case CircleTopicActions.SET_CIRCLE_TOPIC_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        default:
            return state;
    }
}