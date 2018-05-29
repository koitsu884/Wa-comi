import { Action } from "@ngrx/store";
import { AppUser } from "../../_models/AppUser";
import { Member } from "../../_models/Member";
import { BusinessUser } from "../../_models/BusinessUser";
import { Blog } from "../../_models/Blog";
import { AuthUser } from "../../_models/AuthUser";

export const TRY_LOGIN = 'TRY_LOGIN';
export const LOGIN = 'LOGIN';
export const TRY_SIGNUP = 'TRY_SIGNUP';
export const SIGNUP_SUCCESS = 'SIGNUP_SUCCESS';
export const LOGOUT = 'LOGOUT'
export const SET_TOKEN = 'SET_TOKEN';
export const TOKEN_EXPIRED = 'TOKEN_EXPIRED';
export const SET_APPUSER = 'SET_APPUSER';
export const GET_MEMBER = 'GET_MEMBER';
export const SET_MEMBER = 'SET_MEMBER';
export const GET_BISUSER = 'GET_BISUSER';
export const SET_BISUSER = 'SET_BISUSER';

export const UPDATE_APPUSER = 'UPDATE_APPUSER';
export const UPDATE_MEMBER = 'UPDATE_MEMBER';
export const UPDATE_BISUSER = 'UPDATE_BISUSER';

export const TRY_ADDBLOG = 'TRY_ADDBLOG';
export const TRY_DELETEBLOG = 'TRY_DELETEBLOG';

export const GET_BLOG = 'GET_BLOG';
export const SET_BLOG = 'SET_BLOG';
export const ADD_BLOG = 'ADD_BLOG';
export const DELETE_BLOG = 'DELETE_BLOG';

export class TrySignup implements Action{
    readonly type = TRY_SIGNUP;

    constructor(public payload: {registerInfo: AppUser}){}
}

export class SignUpSuccess implements Action {
    readonly type = SIGNUP_SUCCESS;

    constructor(public payload: string){}
}

export class TryLogin implements Action{
    readonly type = TRY_LOGIN;

    constructor(public payload: {UserName: string, Password: string}){}
}

export class Login implements Action {
    readonly type = LOGIN;
    constructor(public payload: {appUser: AppUser, token: string}){};
}

export class Logout implements Action {
    readonly type = LOGOUT;
}

export class SetToken implements Action {
    readonly type = SET_TOKEN;

    constructor(public payload: {token: string, appUser: AppUser}) {}
}

export class TokenExpired implements Action {
    readonly type = TOKEN_EXPIRED;

    constructor() {}
}

export class SetAppUser implements Action {
    readonly type = SET_APPUSER;

    constructor(public payload: AppUser) {}
}

export class GetMember implements Action {
    readonly type = GET_MEMBER;

    constructor(public payload: number) {}
}

export class SetMember implements Action {
    readonly type = SET_MEMBER;

    constructor(public payload: Member) {}
}

export class GetBisUser implements Action {
    readonly type = GET_BISUSER;

    constructor(public payload: number) {}
}

export class SetBisUser implements Action {
    readonly type = SET_BISUSER;

    constructor(public payload: BusinessUser) {}
}

export class UpdateAppUser implements Action {
    readonly type = UPDATE_APPUSER;

    constructor(public payload: AppUser) {}
}


export class UpdateMember implements Action {
    readonly type = UPDATE_MEMBER;

    constructor(public payload: Member) {}
}

export class UpdateBisUser implements Action {
    readonly type = UPDATE_BISUSER;

    constructor(public payload: BusinessUser) {}
}

export class TryAddBlog implements Action {
    readonly type = TRY_ADDBLOG;

    constructor(public payload:{type:string, recordId:number}){}
}

export class TryDeleteBlog implements Action {
    readonly type = TRY_DELETEBLOG;

    constructor(public payload: {type:string, recordId:number, id:number}) {}
}

export class GetBlog implements Action {
    readonly type = GET_BLOG;
    constructor(public payload: {type:string, recordId:number}) {}
}

export class SetBlog implements Action {
    readonly type = SET_BLOG;
    constructor(public payload: {blogs: Blog[]}) {}
}

export class AddBlog implements Action {
    readonly type = ADD_BLOG;
    constructor(public payload:Blog) {}
}

export class DeleteBlog implements Action {
    readonly type = DELETE_BLOG;

    constructor(public payload: number) {}
}


export type AccountActions = TryLogin 
                        | Login
                        | TrySignup 
                        | SignUpSuccess
                        | Logout 
                        | SetToken 
                        | SetAppUser
                        | TokenExpired
                        | GetMember
                        | SetMember
                        | GetBisUser
                        | SetBisUser
                        | UpdateAppUser
                        | UpdateMember
                        | UpdateBisUser
                        | SetBlog
                        | AddBlog
                        | DeleteBlog
                        ;