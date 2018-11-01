import { Action } from "@ngrx/store";
import { CircleEventParticipation } from "../../_models/CircleEventParticipation";
import { Pagination } from "../../_models/Pagination";

export const GET_CIRCLE_EVENT_CONFIRMEDLIST = 'GET_CIRCLE_EVENT_CONFIRMEDLIST';
export const SET_CIRCLE_EVENT_CONFIRMEDLIST = 'SET_CIRCLE_EVENT_CONFIRMEDLIST';
export const GET_CIRCLE_EVENT_WAITLIST = 'GET_CIRCLE_EVENT_WAITLIST';
export const SET_CIRCLE_EVENT_WAITLIST = 'SET_CIRCLE_EVENT_WAITLIST';
export const GET_CIRCLE_EVENT_CANCELEDLIST = 'GET_CIRCLE_EVENT_CANCELEDLIST';
export const SET_CIRCLE_EVENT_CANCELEDLIST = 'SET_CIRCLE_EVENT_CANCELEDLIST';
export const APPROVE_EVENT_PARTICIPATION_REQUEST = 'APPROVE_EVENT_PARTICIPATION_REQUEST';

export class GetCircleEventConfirmedList implements Action {
    readonly type = GET_CIRCLE_EVENT_CONFIRMEDLIST;
    constructor(public payload: {eventId: number, pageNumber?: number}){}
}

export class SetCircleEventConfirmedList implements Action {
    readonly type = SET_CIRCLE_EVENT_CONFIRMEDLIST;
    constructor(public payload: {participants: CircleEventParticipation[], pagination: Pagination}){}
}

export class GetCircleEventWaitList implements Action {
    readonly type = GET_CIRCLE_EVENT_WAITLIST;
    constructor(public payload: {eventId: number, pageNumber?: number}){}
}

export class SetCircleEventWaitList implements Action {
    readonly type = SET_CIRCLE_EVENT_WAITLIST;
    constructor(public payload: {participants: CircleEventParticipation[], pagination: Pagination}){}
}

export class GetCircleEventCanceledList implements Action {
    readonly type = GET_CIRCLE_EVENT_CANCELEDLIST;
    constructor(public payload: {eventId: number, pageNumber?: number}){}
}

export class SetCircleEventCanceledList implements Action {
    readonly type = SET_CIRCLE_EVENT_CANCELEDLIST;
    constructor(public payload: {participants: CircleEventParticipation[], pagination: Pagination}){}
}

export class ApproveEventParticipationRequest implements Action {
    readonly type = APPROVE_EVENT_PARTICIPATION_REQUEST;
    constructor(public payload: {appUserId:number, circleEventId: number}) { }
}

export type CircleEventParticipationsActions = GetCircleEventConfirmedList
                                             | SetCircleEventConfirmedList
                                             | GetCircleEventWaitList
                                             | SetCircleEventWaitList
                                             | GetCircleEventCanceledList
                                             | SetCircleEventCanceledList
                                             | ApproveEventParticipationRequest;