import { Action } from "@ngrx/store";
import { DailyTopic } from "../../_models/DailyTopic";

export const GET_TOPIC_LIST = 'GET_TOPIC_LIST';
export const SET_TOPIC_LIST = 'SET_TOPIC_LIST';
export const TRY_ADD_TOPIC = 'TRY_ADD_TOPIC';
export const ADD_TOPIC = 'ADD_TOPIC';
export const TRY_DELETE_TOPIC = 'TRY_DELETE_TOPIC';
export const DELETE_TOPIC = 'DELETE_TOPIC';
export const LIKE_TOPIC = 'LIKE_TOPIC';

export class GetTopicList implements Action {
    readonly type = GET_TOPIC_LIST;
    constructor(public payload:string) {} //userId
}

export class SetTopicList implements Action {
    readonly type = SET_TOPIC_LIST;
    constructor(public payload: DailyTopic[]) {}
}

export class TryAddTopic implements Action {
    readonly type = TRY_ADD_TOPIC;
    constructor(public payload: {userId: string, title: string}){}
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

export class LikeTopic implements Action {
    readonly type = LIKE_TOPIC;
    constructor(public payload: {supportUserId: string, dailyTopicId:number}){}
}

export type DailyTopicActions = GetTopicList 
                          | SetTopicList
                          | TryAddTopic
                          | AddTopic
                          | TryDeleteTopic
                          | DeleteTopic
                          | LikeTopic;