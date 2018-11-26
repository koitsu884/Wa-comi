import { Action } from "@ngrx/store";
import { UserComment } from "../../../_models/UserComment";
import { Pagination } from "../../../_models/Pagination";
import { ShortComment } from "../../../_models/ShortComment";

export const GET_USER_COMMENT_LIST = 'GET_USER_COMMENT_LIST';
export const SET_USER_COMMENT_LIST = 'SET_USER_COMMENT_LIST';
export const GET_FORCUSED_COMMENT = 'GET_FORCUSED_COMMENT';
export const TOGGLE_USER_REPLY_FORM = 'TOGGLE_USER_REPLY_FORM';
export const GET_USER_REPLIES = 'GET_USER_REPLIES';
export const SET_USER_REPLIES = 'SET_USER_REPLIES';
export const ADD_USER_COMMENT = 'ADD_USER_COMMENT';
export const SET_USER_COMMENT_PAGE = 'SET_USER_COMMENT_PAGE';
export const TRY_ADD_USER_REPLY = 'TRY_ADD_USER_REPLY';


export class GetUserCommentList implements Action {
    readonly type = GET_USER_COMMENT_LIST;
    constructor(public payload: { ownerRecordType:string, ownerRecordId: number, pageNumber?: number}) { }
}

export class SetUserCommentList implements Action {
    readonly type = SET_USER_COMMENT_LIST;
    constructor(public payload:{ commentList: UserComment[], pagination: Pagination}){}
}

export class GetForcusedUserComment implements Action {
    readonly type = GET_FORCUSED_COMMENT;
    constructor(public payload: {ownerRecordType:string, ownerRecordId: number, id: number}) { }
}

export class ToggleUserReplyForm implements Action {
    readonly type = TOGGLE_USER_REPLY_FORM;
    constructor(public payload: number){}
}

export class GetUserReplies implements Action {
    readonly type = GET_USER_REPLIES;
    constructor(public payload: { ownerRecordType:string, commentId: number}) { }
}

export class SetUserReplies implements Action {
    readonly type = SET_USER_REPLIES;
    constructor(public payload:{  ownerRecordType:number, ownerRecordId: number, userReplies: ShortComment[]}){}
}

export class AddUserComment implements Action {
    readonly type = ADD_USER_COMMENT;
    constructor(public payload: UserComment) { }
}

export class SetUserCommentPage implements Action {
    readonly type = SET_USER_COMMENT_PAGE;
    constructor(public payload: number) { }
}


export type CommentActions = GetUserCommentList
                        | SetUserCommentList
                        | GetForcusedUserComment
                        | ToggleUserReplyForm
                        | GetUserReplies
                        | SetUserReplies
                        | AddUserComment
                        | SetUserCommentPage;