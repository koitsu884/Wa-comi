import { Action } from "@ngrx/store";
import { CircleTopic } from "../../_models/CircleTopic";
import { Pagination } from "../../_models/Pagination";
import { CircleTopicComment } from "../../_models/CircleTopicComment";
import { ShortComment } from "../../_models/ShortComment";

export const GET_CIRCLE_TOPIC = 'GET_CIRCLE_TOPIC';
export const SET_CIRCLE_TOPIC = 'SET_CIRCLE_TOPIC';
export const GET_CIRCLE_TOPIC_COMMENT_LIST = 'GET_CIRCLE_TOPIC_COMMENT_LIST';
export const SET_CIRCLE_TOPIC_COMMENT_LIST = 'SET_CIRCLE_TOPIC_COMMENT_LIST';
export const TOGGLE_CIRCLE_TOPIC_REPLY_FORM = 'TOGGLE_CIRCLE_TOPIC_REPLY_FORM';
export const GET_CIRCLE_TOPIC_REPLIES = 'GET_CIRCLE_TOPIC_REPLIES';
export const SET_CIRCLE_TOPIC_REPLIES = 'SET_CIRCLE_TOPIC_REPLIES';
export const UPDATE_CIRCLE_TOPIC = 'UPDATE_CIRCLE_TOPIC';
export const GET_CIRCLE_TOPIC_LIST = 'GET_CIRCLE_TOPIC_LIST';
export const SET_CIRCLE_TOPIC_LIST = 'SET_CIRCLE_TOPIC_LIST';
export const SET_CIRCLE_TOPIC_PAGE = 'SET_CIRCLE_TOPIC_PAGE';
export const DELETE_CIRCLE_TOPIC = 'DELETE_CIRCLE_TOPIC';
export const ADD_CIRCLE_TOPIC_COMMENT = 'ADD_CIRCLE_TOPIC_COMMENT';
export const SET_CIRCLE_TOPIC_COMMENT_PAGE = 'SET_CIRCLE_TOPIC_COMMENT_PAGE';
export const TRY_ADD_CIRCLE_TOPIC_REPLY = 'TRY_ADD_CIRCLE_TOPIC_REPLY';

export class GetCircleTopic implements Action {
    readonly type = GET_CIRCLE_TOPIC;
    constructor(public payload: number) { }
}

export class SetCircleTopic implements Action {
    readonly type = SET_CIRCLE_TOPIC;
    constructor(public payload: CircleTopic) { }
}

export class GetCircleTopicCommentList implements Action {
    readonly type = GET_CIRCLE_TOPIC_COMMENT_LIST;
    constructor(public payload: { topicId: number, initPage?: boolean}) { }
}

export class SetCircleTopicCommentList implements Action {
    readonly type = SET_CIRCLE_TOPIC_COMMENT_LIST;
    constructor(public payload:{ commentList: CircleTopicComment[], pagination: Pagination}){}
}

export class ToggleCircleTopicReplyForm implements Action {
    readonly type = TOGGLE_CIRCLE_TOPIC_REPLY_FORM;
    constructor(public payload: number){}
}

export class GetCircleTopicReplies implements Action {
    readonly type = GET_CIRCLE_TOPIC_REPLIES;
    constructor(public payload: number) { }
}

export class SetCircleTopicReplies implements Action {
    readonly type = SET_CIRCLE_TOPIC_REPLIES;
    constructor(public payload:{ commentId: number, topicReplies: ShortComment[]}){}
}

export class UpdateCircleTopic implements Action {
    readonly type = UPDATE_CIRCLE_TOPIC;

    constructor(public payload:{circleTopic: CircleTopic, photo:File}) {}
}




export class GetCircleTopicList implements Action {
    readonly type = GET_CIRCLE_TOPIC_LIST;
    constructor(public payload: { circleId: number, initPage?: boolean}) { }
}

export class SetCircleTopicList implements Action {
    readonly type = SET_CIRCLE_TOPIC_LIST;
    constructor(public payload: { topicList:CircleTopic[], pagination: Pagination}) { }
}

export class SetCircleTopicPage implements Action {
    readonly type = SET_CIRCLE_TOPIC_PAGE;
    constructor(public payload: number) { }
}

export class AddCircleTopicComment implements Action {
    readonly type = ADD_CIRCLE_TOPIC_COMMENT;
    constructor(public payload: CircleTopicComment) { }
}

export class SetCircleTopicCommentPage implements Action {
    readonly type = SET_CIRCLE_TOPIC_COMMENT_PAGE;
    constructor(public payload: number) { }
}


export type CircleTopicActions = GetCircleTopic
                        | SetCircleTopic
                        | UpdateCircleTopic
                        | GetCircleTopicList
                        | SetCircleTopicList
                        | SetCircleTopicPage
                        | GetCircleTopicCommentList
                        | SetCircleTopicCommentList
                        | ToggleCircleTopicReplyForm
                        | GetCircleTopicReplies
                        | SetCircleTopicReplies
                        | AddCircleTopicComment
                        | SetCircleTopicCommentPage;