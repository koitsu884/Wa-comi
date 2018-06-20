import { Action } from "@ngrx/store";
import { AppUser } from "../../_models/AppUser";
import { Blog } from "../../_models/Blog";
import { RegisterInfo } from "../../_models/RegisterInfo";
import { LoginResult } from "../../_models/LoginResult";
import { MemberProfile } from "../../_models/MemberProfile";
import { BusinessProfile } from "../../_models/BusinessProfile";
import { UserAccount } from "../../_models/UserAccount";

export const TRY_LOGIN = 'TRY_LOGIN';
export const LOGIN = 'LOGIN';
export const TRY_SIGNUP = 'TRY_SIGNUP';
export const SIGNUP_SUCCESS = 'SIGNUP_SUCCESS';
export const LOGOUT = 'LOGOUT'
export const SET_TOKEN = 'SET_TOKEN';
export const TOKEN_EXPIRED = 'TOKEN_EXPIRED';
export const GET_APPUSER = 'GET_APPUSER';
export const SET_APPUSER = 'SET_APPUSER';
export const GET_ACCOUNT = 'GET_ACCOUNT';
export const SET_ACCOUNT = 'SET_ACCOUNT';
export const GET_MEMBER_PROFILE = 'GET_MEMBER_PROFILE';
export const SET_MEMBER_PROFILE = 'SET_MEMBER_PROFILE';
export const GET_BUSINESS_PROFILE = 'GET_BUSINESS_PROFILE';
export const SET_BUSINESS_PROFILE = 'SET_BUSINESS_PROFILE';
export const SET_ADMIN_FLAG = 'SET_ADMIN_FLAG';

export const UPDATE_APPUSER = 'UPDATE_APPUSER';
export const UPDATE_ACCOUNT = 'UPDATE_ACCOUNT';
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

    constructor(public payload: {registerInfo: RegisterInfo}){}
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
    constructor(public payload: LoginResult){};
}

export class Logout implements Action {
    readonly type = LOGOUT;
}

export class SetToken implements Action {
    readonly type = SET_TOKEN;

    constructor(public payload:string) {}
}

export class SetAdminFlag implements Action {
    readonly type = SET_ADMIN_FLAG;

    constructor(public payload:boolean) {}
}

export class TokenExpired implements Action {
    readonly type = TOKEN_EXPIRED;

    constructor() {}
}

export class GetAccount implements Action {
    readonly type = GET_ACCOUNT;

    constructor(public payload: string) {}
}

export class SetAccount implements Action {
    readonly type = SET_ACCOUNT;

    constructor(public payload: UserAccount) {}
}

export class GetAppUser implements Action {
    readonly type = GET_APPUSER;

    constructor(public payload: number) {}
}

export class SetAppUser implements Action {
    readonly type = SET_APPUSER;

    constructor(public payload: AppUser) {}
}

export class GetMemberProfile implements Action {
    readonly type = GET_MEMBER_PROFILE;

    constructor(public payload: number) {}
}

export class SetMemberProfile implements Action {
    readonly type = SET_MEMBER_PROFILE;

    constructor(public payload: MemberProfile) {}
}

export class GetBusinessProfile implements Action {
    readonly type = GET_BUSINESS_PROFILE;

    constructor(public payload: number) {}
}

export class SetBusinessProfile implements Action {
    readonly type = SET_BUSINESS_PROFILE;

    constructor(public payload: BusinessProfile) {}
}

export class UpdateAppUser implements Action {
    readonly type = UPDATE_APPUSER;

    constructor(public payload: AppUser) {}
}

export class UpdateAccount implements Action {
    readonly type = UPDATE_ACCOUNT;

    constructor(public payload: UserAccount) {}
}


export class UpdateMember implements Action {
    readonly type = UPDATE_MEMBER;

    constructor(public payload: MemberProfile) {}
}

export class UpdateBisUser implements Action {
    readonly type = UPDATE_BISUSER;

    constructor(public payload: BusinessProfile) {}
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
                        | SetAdminFlag
                        | GetAccount
                        | SetAccount
                        | GetAppUser
                        | SetAppUser
                        | TokenExpired
                        | GetMemberProfile
                        | SetMemberProfile
                        | GetBusinessProfile
                        | SetBusinessProfile
                        | UpdateAppUser
                        | UpdateAccount
                        | UpdateMember
                        | UpdateBisUser
                        | SetBlog
                        | AddBlog
                        | DeleteBlog
                        ;