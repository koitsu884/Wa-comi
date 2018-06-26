import { Action } from "@ngrx/store";
import { Blog } from "../../_models/Blog";

export const GET_BLOG = 'GET_BLOG';
export const SET_BLOG = 'SET_BLOG';
export const TRY_ADDBLOG = 'TRY_ADDBLOG';
export const TRY_DELETEBLOG = 'TRY_DELETEBLOG';
export const ADD_BLOG = 'ADD_BLOG';
export const UPDATE_BLOG = 'UPDATE_BLOG';
export const DELETE_BLOG = 'DELETE_BLOG';
export const CLEAR_BLOG = 'CLEAR_BLOG';
export const SET_SELECTED_BLOG = 'SET_SELECTED_BLOG';



export class TryAddBlog implements Action {
    readonly type = TRY_ADDBLOG;

    constructor(public payload:Blog){}
}

export class TryDeleteBlog implements Action {
    readonly type = TRY_DELETEBLOG;

    constructor(public payload: number) {}
}

export class GetBlog implements Action {
    readonly type = GET_BLOG;
    constructor(public payload: number) {}
}

export class SetBlog implements Action {
    readonly type = SET_BLOG;
    constructor(public payload: Blog[]) {}
}

export class SetSelectedBlog implements Action {
    readonly type = SET_SELECTED_BLOG;
    constructor(public payload: Blog) {}
}

export class AddBlog implements Action {
    readonly type = ADD_BLOG;
    constructor(public payload:Blog) {}
}

export class UpdateBlog implements Action {
    readonly type = UPDATE_BLOG;

    constructor(public payload: Blog) {}
}

export class DeleteBlog implements Action {
    readonly type = DELETE_BLOG;

    constructor(public payload: number) {}
}

export class ClearBlog implements Action {
    readonly type = CLEAR_BLOG;

    constructor() {}
}


export type AccountActions = TryAddBlog | TryDeleteBlog | GetBlog | SetBlog | AddBlog | UpdateBlog | DeleteBlog | ClearBlog;