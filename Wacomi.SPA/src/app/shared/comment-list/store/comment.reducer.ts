import { UserComment } from "../../../_models/UserComment";
import { Pagination } from "../../../_models/Pagination";
import * as CommentActions from './comment.actions';

export interface State {
    userCommentList: UserComment[];
    pagination: Pagination;
}

const initialState: State = {
    userCommentList: [],
    pagination: null
};

export function commentReducer(state = initialState, action: CommentActions.CommentActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case CommentActions.GET_USER_COMMENT_LIST:
            return {
                ...state,
                userCommentList: [],
            }

        case CommentActions.SET_USER_COMMENT_LIST:
            return {
                ...state,
                userCommentList: action.payload.commentList.reverse(),
                pagination: action.payload.pagination
            }
        case CommentActions.ADD_USER_COMMENT:
            return {
                ...state,
                userCommentList: [...state.userCommentList, action.payload]
            }
        case CommentActions.SET_USER_COMMENT_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        case CommentActions.TOGGLE_USER_REPLY_FORM:
            var tempUserComments = [...state.userCommentList];
            var index = tempUserComments.findIndex(x => x.id == action.payload);
            tempUserComments[index].displayReplies = !tempUserComments[index].displayReplies;
            return {
                ...state,
                userCommentList: tempUserComments
            }
        case CommentActions.SET_USER_REPLIES:
            tempUserComments = [...state.userCommentList];
            var index = tempUserComments.findIndex(x => x.id == action.payload.ownerRecordId);
            tempUserComments[index].replies = action.payload.userReplies;
            tempUserComments[index].replyCount = action.payload.userReplies.length;

            return {
                ...state,
                topicComments: tempUserComments
            }
        default:
            return state;
    }
}