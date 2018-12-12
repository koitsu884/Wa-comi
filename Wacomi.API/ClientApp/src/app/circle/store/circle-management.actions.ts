import { Action } from "@ngrx/store";
import { Circle } from "../../_models/Circle";
import { Pagination } from "../../_models/Pagination";

export const GET_MY_CIRCLE_LIST = 'GET_MY_CIRCLE_LIST';
export const SET_MY_CIRCLE_LIST = 'SET_MY_CIRCLE_LIST';
export const SET_MY_CIRCLE_PAGE = 'SET_MY_CIRCLE_PAGE';
export const GET_OWN_CIRCLE_LIST = 'GET_OWN_CIRCLE_LIST';
export const SET_OWN_CIRCLE_LIST = 'SET_OWN_CIRCLE_LIST';
export const SET_OWN_CIRCLE_PAGE = 'SET_OWN_CIRCLE_PAGE';

export class GetMyCircleList implements Action {
    readonly type = GET_MY_CIRCLE_LIST;
    constructor(public payload: { userId: number, page: number }) { }
}

export class SetMyCircleList implements Action {
    readonly type = SET_MY_CIRCLE_LIST;
    constructor(public payload: { circles: Circle[], pagination: Pagination }) { }
}

export class SetMyCirclePage implements Action {
    readonly type = SET_MY_CIRCLE_PAGE;
    constructor(public payload: {userId:number, page:number}) { }
}

export class GetOwnCircleList implements Action {
    readonly type = GET_OWN_CIRCLE_LIST;
    constructor(public payload: { userId: number, page: number }) { }
}

export class SetOwnCircleList implements Action {
    readonly type = SET_OWN_CIRCLE_LIST;
    constructor(public payload: { circles: Circle[], pagination: Pagination }) { }
}

export class SetOwnCirclePage implements Action {
    readonly type = SET_OWN_CIRCLE_PAGE;
    constructor(public payload: {userId:number, page:number}) { }
}

export type CircleManagementActions = GetMyCircleList
                        | SetMyCircleList
                        | SetMyCirclePage
                        | GetOwnCircleList
                        | SetOwnCircleList
                        | SetOwnCirclePage;