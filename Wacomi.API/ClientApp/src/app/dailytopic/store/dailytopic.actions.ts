import { Action } from "@ngrx/store";
import { DailyTopic } from "../../_models/DailyTopic";
import { TopicComment } from "../../_models/TopicComment";
import { TopicCommentFeel } from "../../_models/TopicCommentFeel";
import { importExpr } from "@angular/compiler/src/output/output_ast";
import { TopicReply } from "../../_models/TopicReply";
import { ShortComment } from "../../_models/ShortComment";

//====================================================
// Daily Topic Ranking
//====================================================
export const GET_TOPIC_LIST = 'GET_TOPIC_LIST';
export const SET_TOPIC_LIST = 'SET_TOPIC_LIST';
export const TRY_ADD_TOPIC = 'TRY_ADD_TOPIC';
export const ADD_TOPIC = 'ADD_TOPIC';
export const TRY_DELETE_TOPIC = 'TRY_DELETE_TOPIC';
export const DELETE_TOPIC = 'DELETE_TOPIC';
export const GET_LIKED_TOPIC_LIST = 'GET_LIKED_TOPIC_LIST';
export const SET_LIKED_TOPIC_LIST = 'SET_LIKED_TOPIC_LIST';

export const LIKE_TOPIC = 'LIKE_TOPIC';

export const TOPIC_CLEAR = 'TOPIC_CLEAR';

export class GetTopicList implements Action {
    readonly type = GET_TOPIC_LIST;
    constructor(public payload:number) {} //appUserId
}

export class SetTopicList implements Action {
    readonly type = SET_TOPIC_LIST;
    constructor(public payload: DailyTopic[]) {}
}

export class TryAddTopic implements Action {
    readonly type = TRY_ADD_TOPIC;
    constructor(public payload: {userId: number, title: string}){}
}

export class AddTopic implements Action {
    readonly type = ADD_TOPIC;
    constructor(public payload: DailyTopic){}
}

export class TryDeleteTopic implements Action {
    readonly type = TRY_DELETE_TOPIC;
    constructor(public payload: {userId: string, id: number}){}
}

export class DeleteTopic implements Action {
    readonly type = DELETE_TOPIC;
    constructor(public payload: number) {}
}

export class GetLikedTopicList implements Action {
    readonly type = GET_LIKED_TOPIC_LIST;
    constructor(public payload:number) {} //userId
}

export class SetLikedTopicList implements Action {
    readonly type = SET_LIKED_TOPIC_LIST;
    constructor(public payload: number[]) {}
}

export class LikeTopic implements Action {
    readonly type = LIKE_TOPIC;
    constructor(public payload: {supportAppUserId: number, dailyTopicId:number}){}
}

export class TopicClear implements Action {
    readonly type = TOPIC_CLEAR;
}

//====================================================
// Daily Topic Comments
//====================================================
export const GET_TOPIC_COMMENTS = 'GET_TOPIC_COMMENTS';
export const SET_TOPIC_COMMENTS = 'SET_TOPIC_COMMENTS';
export const TRY_ADD_TOPIC_COMMENT = 'TRY_ADD_TOPIC_COMMENT';
export const ADD_TOPIC_COMMENT = 'ADD_TOPIC_COMMENT';
export const TRY_DELETE_TOPIC_COMMENT = 'TRY_DELETE_TOPIC_COMMENT';
export const DELETE_TOPIC_COMMENT = 'DELETE_TOPIC_COMMENT';

export const TOGGLE_REPLY_FORM = "TOGGLE_REPLY_FORM";
export const GET_TOPIC_REPLIES = 'GET_TOPIC_REPLIES';
export const SET_TOPIC_REPLIES = 'SET_TOPIC_REPLIES';
export const TRY_ADD_TOPIC_REPLY = 'TRY_ADD_TOPIC_REPLY';
// export const ADD_TOPIC_REPLY = 'ADD_TOPIC_REPLY';
export const TRY_DELETE_TOPIC_REPLY = 'TRY_DELETE_TOPIC_REPLY';
export const DELETE_TOPIC_REPLY = 'DELETE_TOPIC_REPLY';

export const GET_COMMENT_FEELINGS = 'GET_COMMENT_FEELINGS';
export const SET_COMMENT_FEELINGS = 'SET_COMMENT_FEELINGS';
export const TRY_ADD_COMMENT_FEELING = 'TRY_ADD_COMMENT_FEELING';
export const ADD_COMMENT_FEELING = 'ADD_COMMENT_FEELING';

export class GetTopicComments implements Action {
    readonly type = GET_TOPIC_COMMENTS;
    constructor(public payload: number) {}// appUserId
}

export class SetTopicComments implements Action {
    readonly type = SET_TOPIC_COMMENTS;
    constructor(public payload: {comments: TopicComment[], appUserId: number}) {}
}

export class TryAddTopicComment implements Action {
    readonly type = TRY_ADD_TOPIC_COMMENT;
    constructor(public payload: {appUserId: number, topicTitle: string, comment: string}){}
}

export class AddTopicComment implements Action {
    readonly type = ADD_TOPIC_COMMENT;
    constructor(public payload: TopicComment){}
}

export class TryDeleteTopicComment implements Action {
    readonly type = TRY_DELETE_TOPIC_COMMENT;
    constructor(public payload: number){} //CommentId
}

export class DeleteTopicComment implements Action {
    readonly type = DELETE_TOPIC_COMMENT;
    constructor(public payload: number) {}
}

export class GetCommentFeelings implements Action {
    readonly type = GET_COMMENT_FEELINGS;
    constructor(public payload: number){} //appUserId
}

export class SetCommentFeelings implements Action {
    readonly type = SET_COMMENT_FEELINGS;
    constructor(public payload: TopicCommentFeel[]){}
}

export class TryAddCommentFeeling implements Action {
    readonly type = TRY_ADD_COMMENT_FEELING;
    constructor(public payload: TopicCommentFeel){}
}

export class AddCommentFeeling implements Action {
    readonly type = ADD_COMMENT_FEELING;
    constructor(public payload: TopicCommentFeel){}
}

export class ToggleReplyForm implements Action {
    readonly type = TOGGLE_REPLY_FORM;
    constructor(public payload: {commentId: number}){}
}

export class GetTopicReplies implements Action {
    readonly type = GET_TOPIC_REPLIES;
    constructor(public payload: {commentId: number}){}
}

export class SetTopicReplies implements Action {
    readonly type = SET_TOPIC_REPLIES;
    constructor(public payload:{commentId: number, topicReplies: ShortComment[]}){}
}

export class TryAddTopicReply implements Action {
    readonly type = TRY_ADD_TOPIC_REPLY;
    constructor(public payload:{topicCommentId: number, appUserId: number, reply: string }){}
}

// export class AddTopicReply implements Action {
//     readonly type = ADD_TOPIC_REPLY;
//     constructor(public payload:ShortComment){}
// }

export class TryDeleteTopicReply implements Action {
    readonly type = TRY_DELETE_TOPIC_REPLY;
    constructor(public payload:{topicReplyId: number, topicCommentId: number}){}
}

export class DeleteTopicReply implements Action {
    readonly type = DELETE_TOPIC_REPLY;
    constructor(public payload:TopicReply){}
}


export type DailyTopicActions = GetTopicList 
                          | SetTopicList
                          | TryAddTopic
                          | AddTopic
                          | TryDeleteTopic
                          | DeleteTopic
                          | GetLikedTopicList
                          | SetLikedTopicList
                          | LikeTopic
                          | GetTopicComments
                          | SetTopicComments
                          | TryAddTopicComment
                          | AddTopicComment
                          | TryDeleteTopicComment
                          | DeleteTopicComment
                          | GetCommentFeelings
                          | SetCommentFeelings
                          | TryAddCommentFeeling
                          | AddCommentFeeling
                          | ToggleReplyForm
                          | GetTopicReplies
                          | SetTopicReplies
                          | TryAddTopicReply
                        //   | AddTopicReply
                          | TryDeleteTopicReply
                          | DeleteTopicReply
                          | TopicClear;