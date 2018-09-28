import * as CircleTopicActions from '../store/circletopic.actions';
import { CircleTopic } from "../../_models/CircleTopic";
import { Pagination } from "../../_models/Pagination";
import { CircleTopicComment } from '../../_models/CircleTopicComment';

export interface State {
    latestTopicList: CircleTopic[];
    topicList: CircleTopic[];
    pagination: Pagination;
    selectedCircleTopic: CircleTopic;
    circleTopicCommentList: CircleTopicComment[];
    commentPagination: Pagination;
}

const wtfInitialState: State = {
    latestTopicList: [],
    topicList: [],
    pagination: null,
    selectedCircleTopic: null,
    circleTopicCommentList: [],
    commentPagination: null
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
        case CircleTopicActions.GET_CIRCLE_TOPIC_COMMENT_LIST:
            return {
                ...state,
                circleTopicCommentList: [],
            }

        case CircleTopicActions.SET_CIRCLE_TOPIC_COMMENT_LIST:
            return {
                ...state,
                circleTopicCommentList: action.payload.commentList,
                commentPagination: action.payload.pagination
            }
        case CircleTopicActions.ADD_CIRCLE_TOPIC_COMMENT:
            return {
                ...state,
                circleTopicCommentList: [...state.circleTopicCommentList, action.payload]
            }
        case CircleTopicActions.SET_CIRCLE_TOPIC_COMMENT_PAGE:
            tempPagination = state.commentPagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                commentPagination: tempPagination
            }
        case CircleTopicActions.TOGGLE_CIRCLE_TOPIC_REPLY_FORM:
            var tempTopicComments = [...state.circleTopicCommentList];
            var index = tempTopicComments.findIndex(x => x.id == action.payload);
            tempTopicComments[index].displayReplies = !tempTopicComments[index].displayReplies;
            return {
                ...state,
                circleTopicCommentList: tempTopicComments
            }
        case CircleTopicActions.SET_CIRCLE_TOPIC_REPLIES:
            tempTopicComments = [...state.circleTopicCommentList];
            var index = tempTopicComments.findIndex(x => x.id == action.payload.commentId);
            tempTopicComments[index].topicReplies = action.payload.topicReplies;
            tempTopicComments[index].replyCount = action.payload.topicReplies.length;

            return {
                ...state,
                topicComments: tempTopicComments
            }
        default:
            return state;
    }
}