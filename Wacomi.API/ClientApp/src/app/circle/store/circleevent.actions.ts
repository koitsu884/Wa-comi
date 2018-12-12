import { Action } from "@ngrx/store";
import { CircleEvent } from "../../_models/CircleEvent";
import { CircleEventParticipationStatus, CircleEventParticipation } from "../../_models/CircleEventParticipation";

export const GET_CIRCLE_EVENT = 'GET_CIRCLE_EVENT';
export const SET_CIRCLE_EVENT = 'SET_CIRCLE_EVENT';
export const UPDATE_CIRCLE_EVENT = 'UPDATE_CIRCLE_EVENT';
export const SEARCH_CIRCLE_EVENT = 'SEARCH_CIRCLE_EVENT';
export const SET_CIRCLE_EVENT_SEARCH_PARAMS = 'SET_CIRCLE_EVENT_SEARCH_PARAMS';
export const GET_CIRCLE_EVENT_LIST = 'GET_CIRCLE_EVENT_LIST';
export const ADD_CIRCLE_EVENT_LIST = 'ADD_CIRCLE_EVENT_LIST';
export const LOAD_NEXT_CIRCLE_EVENT_LIST = 'LOAD_NEXT_CIRCLE_EVENT_LIST';
export const SET_EVENT_LOAD_FINISH_FLAG = 'SET_EVENT_LOAD_FINISH_FLAG';
export const SET_EVENT_RELOAD_FLAG = 'SET_EVENT_RELOAD_FLAG';
export const DELETE_CIRCLE_EVENT = 'DELETE_CIRCLE_EVENT';

export const SEND_EVENT_PARTICIPATION_REQUEST = 'SEND_EVENT_PARTICIPATION_REQUEST';
export const SET_EVENT_PARTICIPATION_STATUS = 'SET_EVENT_PARTICIPATION_STATUS';
export const DELETE_EVENT_PARTICIPATION = 'DELETE_EVENT_PARTICIPATION';
export const CANCEL_EVENT_PARTICIPATION = 'CANCEL_EVENT_PARTICIPATION';
export const GET_LATEST_EVENT_PARTICIPANTS = 'GET_LATEST_EVENT_PARTICIPANTS';
export const SET_LATEST_EVENT_PARTICIPANTS = 'SET_LATEST_EVENT_PARTICIPANTS';

export class GetCircleEvent implements Action {
    readonly type = GET_CIRCLE_EVENT;
    constructor(public payload: number) { }
}

export class SetCircleEvent implements Action {
    readonly type = SET_CIRCLE_EVENT;
    constructor(public payload: CircleEvent) { }
}

export class UpdateCircleEvent implements Action {
    readonly type = UPDATE_CIRCLE_EVENT;

    constructor(public payload:{circleEvent: CircleEvent, photo:File}) {}
}

export class SearchCircleEvent implements Action {
    readonly type = SEARCH_CIRCLE_EVENT;
    constructor(public payload: {cityId?: number, circleId?:number, categoryId?: number, fromDate?: Date}) { }
}

export class SetCircleEventSearchParams implements Action {
    readonly type = SET_CIRCLE_EVENT_SEARCH_PARAMS;
    constructor(public payload: {cityId?:number, circleId?:number, categoryId?: number, fromDate?: Date}) { }
}

export class GetCircleEventList implements Action {
    readonly type = GET_CIRCLE_EVENT_LIST;
    constructor(public payload: number) { }//pageNumber
}

export class AddCircleEventList implements Action {
    readonly type = ADD_CIRCLE_EVENT_LIST;
    constructor(public payload: CircleEvent[]) { }
}

export class LoadNextCircleEventList implements Action {
    readonly type = LOAD_NEXT_CIRCLE_EVENT_LIST;
    constructor() { }
}

export class SetEventLoadFinishFlag implements Action {
    readonly type = SET_EVENT_LOAD_FINISH_FLAG;
    constructor() { }
}

export class SetEventReloadFlag implements Action {
    readonly type = SET_EVENT_RELOAD_FLAG;
    constructor() { }
}

//============== Event Participation ================
export class SendEventParticipationRequest implements Action {
    readonly type = SEND_EVENT_PARTICIPATION_REQUEST;
    constructor(public payload: {appUserId:number, circleEventId: number, message:string}) { }
}

export class SetEventParticipationStatus implements Action {
    readonly type = SET_EVENT_PARTICIPATION_STATUS;
    constructor(public payload: CircleEventParticipationStatus){}
}

export class DeleteEventParticipation implements Action {
    readonly type = DELETE_EVENT_PARTICIPATION;
    constructor(public payload: {appUserId:number, circleEventId: number}) { }
}

export class CancelEventParticipation implements Action {
    readonly type = CANCEL_EVENT_PARTICIPATION;
    constructor(public payload: {appUserId:number, circleEventId: number}) { }
}

export class GetLatestEventParticipants implements Action {
    readonly type = GET_LATEST_EVENT_PARTICIPANTS;
    constructor(public payload: number){}
}

export class SetLatestEventParticipants implements Action {
    readonly type = SET_LATEST_EVENT_PARTICIPANTS;
    constructor(public payload: CircleEventParticipation[]){}
}

export type CircleEventActions = SetCircleEventSearchParams
                        | GetCircleEvent
                        | SetCircleEvent
                        | UpdateCircleEvent
                        | SearchCircleEvent
                        | GetCircleEventList
                        | AddCircleEventList
                        | LoadNextCircleEventList
                        | SetEventLoadFinishFlag
                        | SetEventReloadFlag
                        | SendEventParticipationRequest
                        | SetEventParticipationStatus
                        | DeleteEventParticipation
                        | CancelEventParticipation
                        | GetLatestEventParticipants
                        | SetLatestEventParticipants;