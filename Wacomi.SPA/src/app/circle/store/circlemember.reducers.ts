import * as CircleMemberActions from '../store/circlemember.actions';
import { Pagination } from '../../_models/Pagination';
import { CircleMember } from '../../_models/CircleMember';


export interface State {
    memberList: CircleMember[];
    pagination: Pagination;
}

const memberInitialState: State = {
    memberList: [],
    pagination: null
};

export function circleMemberReducer(state = memberInitialState, action: CircleMemberActions.CircleMemberActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case CircleMemberActions.CLEAR_MEMBER_LIST:
            return {
                ...state,
                memberList: [],
                pagination: null
            }
        case CircleMemberActions.GET_CIRCLE_MEMBER_LIST:
            return {
                ...state,
                memberList: [],
            }
        case CircleMemberActions.SET_CIRCLE_MEMBER_LIST:
            return {
                ...state,
                memberList: action.payload.memberList,
                pagination: action.payload.pagination
            }
        case CircleMemberActions.SET_CIRCLE_MEMBER_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        case CircleMemberActions.DELETE_CIRCLE_MEMBER:
            let tempMembers = [...state.memberList];
            let index = tempMembers.findIndex(tm => tm.appUserId == action.payload.appUserId);
            tempMembers.splice(index, 1);
            // let tempCircle = Object.assign({}, state.selectedCircle);
            // tempCircle.isMember = false;
            // if (index != null) {
            //     tempMembers.splice(index, 1);
            // }
            return {
                ...state,
                memberList: tempMembers
            }
        default:
            return state;
    }
}