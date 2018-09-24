import { Action } from "@ngrx/store";
import { Circle } from "../../_models/Circle";
import { Pagination } from "../../_models/Pagination";
import { CircleSearchOptions } from "../../_models/CircleSearchOptions";
import { CircleMember } from "../../_models/CircleMember";
import { CircleRequest } from "../../_models/CircleRequest";

export const GET_CIRCLE = 'GET_CIRCLE';
export const SET_CIRCLE = 'SET_CIRCLE';
export const GET_CIRCLE_REQUEST_LIST = 'GET_CIRCLE_REQUEST_LIST';
export const SET_CIRCLE_REQUEST_LIST = 'SET_CIRCLE_REQUEST_LIST';
export const APPROVE_CIRCLE_REQUEST = 'APPROVE_CIRCLE_REQUEST';
export const DECLINE_CIRCLE_REQUEST = 'DECLINE_CIRCLE_REQUEST';

//============= CIRCLE home & details (search) ======================
export const SET_CIRCLE_SEARCH_OPTIONS = 'SET_CIRCLE_SEARCH_OPTIONS';
export const SET_CIRCLE_PAGE = 'SET_CIRCLE_PAGE';
export const SEARCH_CIRCLE = 'SEARCH_CIRCLE';
export const SET_CIRCLE_SEARCH_RESULT = 'SET_CIRCLE_SEARCH_RESULT';
export const INIT_CIRCLE_STATE = 'INIT_CIRCLE_STATE';

//============= CIRCLE edit =====================
//!!! ALL MOVED TO GLOBAL !!
// export const TRY_ADD_CIRCLE = 'TRY_ADD_CIRCLE';
// export const UPDATE_CIRCLE = 'UPDATE_CIRCLE';
// export const TRY_DELETE_CIRCLE = 'TRY_DELETE_CIRCLE';


export class GetCircle implements Action {
    readonly type = GET_CIRCLE;
    constructor(public payload: number) { }
}

export class SetCircle implements Action {
    readonly type = SET_CIRCLE;
    constructor(public payload: Circle) { }
}

export class GetCircleRequestList implements Action {
    readonly type = GET_CIRCLE_REQUEST_LIST;
    constructor(public payload: number) { }
}

export class SetCircleRequestList implements Action {
    readonly type = SET_CIRCLE_REQUEST_LIST;
    constructor(public payload: CircleRequest[]) { }
}

export class ApproveCircleRequest implements Action {
    readonly type = APPROVE_CIRCLE_REQUEST;
    constructor(public payload: CircleRequest) { }
}

export class DeclineCircleRequest implements Action {
    readonly type = DECLINE_CIRCLE_REQUEST;
    constructor(public payload: CircleRequest) { }
}

export class SetCircleSearchOptions implements Action {
    readonly type = SET_CIRCLE_SEARCH_OPTIONS;
    constructor(public payload: CircleSearchOptions) { }
}

export class SetCirclePage implements Action {
    readonly type = SET_CIRCLE_PAGE;
    constructor(public payload: number) { }
}

export class SearchCircle implements Action {
    readonly type = SEARCH_CIRCLE;
    constructor() { }
}

export class SetCircleSearchResult implements Action {
    readonly type = SET_CIRCLE_SEARCH_RESULT;
    constructor(public payload: { circles: Circle[], pagination: Pagination }) { }
}

export class InitCircleState implements Action {
    readonly type = INIT_CIRCLE_STATE;
    constructor() { }
}

//use global
// export class TryAddCircle implements Action {
//     readonly type = TRY_ADD_CIRCLE;
//     constructor(public payload: { circle: Circle, formData:FormData}) {}
// }

// export class UpdateCircle implements Action {
//     readonly type = UPDATE_CIRCLE;
//     constructor(public payload: CircleEdit){}
// }

// export class TryDeleteCircle implements Action {
//     readonly type = TRY_DELETE_CIRCLE;
//     constructor(public payload: number) {}
// }

export type CircleActions = InitCircleState
                        | GetCircle
                        | SetCircle
                        | GetCircleRequestList
                        | SetCircleRequestList
                        | ApproveCircleRequest
                        | DeclineCircleRequest
                        //   | TryAddCircle
                        //   | UpdateCircle
                        //   | TryDeleteCircle
                        | SetCircleSearchOptions
                        | SetCirclePage
                        | SearchCircle
                        | SetCircleSearchResult;