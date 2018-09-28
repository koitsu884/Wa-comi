import { Action } from "@ngrx/store";
import { CircleMember } from "../../_models/CircleMember";
import { Pagination } from "../../_models/Pagination";


export const GET_CIRCLE_MEMBER_LIST = 'GET_CIRCLE_MEMBER_LIST';
export const SET_CIRCLE_MEMBER_LIST = 'SET_CIRCLE_MEMBER_LIST';
export const SET_CIRCLE_MEMBER_PAGE = 'SET_CIRCLE_MEMBER_PAGE';
export const DELETE_CIRCLE_MEMBER = 'DELETE_CIRCLE_MEMBER';
export const CLEAR_MEMBER_LIST = 'CLEAR_MEMBER_LIST';
export const SEND_CIRCLE_REQUEST = 'SEND_CIRCLE_REQUEST';



export class GetCircleMemberList implements Action {
    readonly type = GET_CIRCLE_MEMBER_LIST;
    constructor(public payload: { circleId: number, initPage?: boolean}) { }
}

export class SetCircleMemberList implements Action {
    readonly type = SET_CIRCLE_MEMBER_LIST;
    constructor(public payload: {memberList:CircleMember[], pagination: Pagination}) { }
}

export class SetCircleMemberPage implements Action {
    readonly type = SET_CIRCLE_MEMBER_PAGE;
    constructor(public payload: number) { }
}

export class DeleteCircleMember implements Action {
    readonly type = DELETE_CIRCLE_MEMBER;
    constructor(public payload: {circleId:number, appUserId:number}) { }
}

export class ClearMemberList implements Action {
    readonly type = CLEAR_MEMBER_LIST;
    constructor() { }
}

export class SendCircleRequest implements Action {
    readonly type = SEND_CIRCLE_REQUEST;
    constructor(public payload: {appUserId: number, circleId: number, requireApproval: boolean, message?: string}) { }
}


export type CircleMemberActions = GetCircleMemberList
                        | SetCircleMemberList
                        | SetCircleMemberPage
                        | DeleteCircleMember
                        | ClearMemberList
                        | SendCircleRequest;