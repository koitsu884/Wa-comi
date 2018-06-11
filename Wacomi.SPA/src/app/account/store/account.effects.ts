import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
// import 'rxjs/add/operator/of';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';

import * as AccountActions from './account.actions';
import * as PhotoActions from '../../photo/store/photos.action';
import * as BlogActions from '../../blog/store/blogs.actions';
import * as GlobalActions from '../../store/global.actions';
import { Router } from "@angular/router";
import { LoginResult } from "../../_models/LoginResult";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { AlertifyService } from "../../_services/alertify.service";
import { Observable } from "rxjs/Observable";

import { Action } from "@ngrx/store";
import { of } from "rxjs/observable/of";
import { AppUser } from "../../_models/AppUser";
import { Blog } from "../../_models/Blog";
import { MemberProfile } from "../../_models/MemberProfile";
import { BusinessProfile } from "../../_models/BusinessProfile";
import { UserAccount } from "../../_models/UserAccount";


@Injectable()
export class AccountEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    //============== Authentication =================
    @Effect()
    authRegister = this.actions$
        .ofType(AccountActions.TRY_SIGNUP)
        .switchMap((action: AccountActions.TrySignup) => {
            return this.httpClient.post(this.baseUrl + 'auth',
                action.payload.registerInfo,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap(() => {
                    return of({
                        type: AccountActions.TRY_LOGIN,
                        payload: {
                            UserName: action.payload.registerInfo.userName,
                            Password: action.payload.registerInfo.password
                        }
                    }
                    )
                }
                )
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        });

    @Effect()
    authSignIn = this.actions$
        .ofType(AccountActions.TRY_LOGIN)
        .switchMap((action: AccountActions.TryLogin) => {
            return this.httpClient.post<LoginResult>(this.baseUrl + 'auth/login', action.payload, { headers: new HttpHeaders().set('Content-Type', 'application/json') })
                .mergeMap((loginResult: LoginResult) => {
                    this.router.navigate(['/home']);
                    return [
                        {
                            type: GlobalActions.SUCCESS,
                            payload: "ログインしました"
                        },
                        {
                            type: AccountActions.LOGIN,
                            payload: loginResult
                        }
                    ];
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: "ログインに失敗しました: " + error })
                });
        });

    @Effect()
    login = this.actions$
            .ofType(AccountActions.LOGIN)
            .map((action: AccountActions.Login) => { return action.payload })
            .mergeMap((loginResult) => {
                this.router.navigate(['/home']);
                return [
                    {
                        type: AccountActions.SET_TOKEN,
                        payload: loginResult.tokenString
                    },
                    {
                        type: AccountActions.SET_APPUSER,
                        payload: loginResult.appUser
                    },
                    {
                        type: AccountActions.SET_ACCOUNT,
                        payload: loginResult.account
                    },
                    {
                        type: PhotoActions.SET_PHOTOS,
                        payload: loginResult.photos
                    },
                    {
                        type: BlogActions.SET_BLOG,
                        payload: loginResult.blogs
                    },
                    {
                        type: AccountActions.SET_MEMBER_PROFILE,
                        payload: loginResult.memberProfile
                    },
                    {
                        type: AccountActions.SET_BUSINESS_PROFILE,
                        payload: loginResult.businessProfile
                    },
                ];
            })


    // @Effect()
    // setToken = this.actions$
    //     .ofType(AccountActions.SET_TOKEN)
    //     .map((action: AccountActions.SetToken) => { return action.payload.appUser})
    //     .map((appUser) => {
    //         switch (appUser.userType) {
    //             case "Member":
    //                 return { type: AccountActions.GET_MEMBER, payload: appUser.userProfileId };
    //             case "Business":
    //                 return { type: AccountActions.GET_BISUSER, payload: appUser.userProfileId };
    //             case "Admin":
    //                 return { type: GlobalActions.SUCCESS, payload: "Admin account"};

    //         }
    //         return { type: GlobalActions.FAILED, payload: "ユーザータイプ'" + appUser.userType + "'は存在しません" };
    //     })

    @Effect()
    getMember = this.actions$
        .ofType(AccountActions.GET_MEMBER_PROFILE)
        .map((action: AccountActions.GetMemberProfile) => { return action.payload })
        .switchMap((id) => {
            return this.httpClient.get<MemberProfile>(this.baseUrl + 'memberprofile/' + id)
            .map((member) => {
                return {
                    type: AccountActions.SET_MEMBER_PROFILE,
                    payload: member
                }
            ;
            })
            .catch((error) => {
                return of({ type: GlobalActions.FAILED, payload: "メンバー情報の取得に失敗しました: " + error })
            })
        })

    @Effect()
    getBisUser = this.actions$
        .ofType(AccountActions.GET_BUSINESS_PROFILE)
        .map((action: AccountActions.GetBusinessProfile) => { return action.payload })
        .switchMap((id) => {
            return this.httpClient.get<BusinessProfile>(this.baseUrl + 'businessprofile/' + id)
            .map((bisUser) => {
                return {
                    type: AccountActions.SET_BUSINESS_PROFILE,
                    payload: bisUser
                }
            })
            .catch((error) => {
                return of({ type: GlobalActions.FAILED, payload: "メンバー情報の取得に失敗しました: " + error })
            })
        })

        @Effect()
        getAccount = this.actions$
            .ofType(AccountActions.GET_ACCOUNT)
            .map((action: AccountActions.GetAccount) => { return action.payload })
            .switchMap((id) => {
                return this.httpClient.get<Account>(this.baseUrl + 'account/' + id)
                .map((account) => {
                    return {
                        type: AccountActions.SET_ACCOUNT,
                        payload: account
                    }
                })
                .catch((error) => {
                    return of({ type: GlobalActions.FAILED, payload: "アカウント情報の取得に失敗しました: " + error })
                })
            })
            @Effect()
            getAppUser = this.actions$
                .ofType(AccountActions.GET_APPUSER)
                .map((action: AccountActions.GetAppUser) => { return action.payload })
                .switchMap((id) => {
                    return this.httpClient.get<AppUser>(this.baseUrl + 'appuser/' + id)
                    .map((appUser) => {
                        return {
                            type: AccountActions.SET_APPUSER,
                            payload: appUser
                        }
                    })
                    .catch((error) => {
                        return of({ type: GlobalActions.FAILED, payload: "ユーザー情報の更新に失敗しました: " + error })
                    })
                })
    //============== Update user information =================
    @Effect()
    updateAccount = this.actions$
        .ofType(AccountActions.UPDATE_ACCOUNT)
        .map((action: AccountActions.UpdateAccount) => {
            return action.payload
        })
        .switchMap((userAccount) => {
            return this.httpClient.put(this.baseUrl + 'account/' + userAccount.id,
            userAccount,
            {
                headers: new HttpHeaders().set('Content-Type', 'application/json')
            })
            .map((account:UserAccount) => {
                this.alertify.success("更新しました");
                return {
                    // type: AccountActions.SET_ACCOUNT, payload: userAccount
                     type: AccountActions.GET_ACCOUNT, payload: userAccount.id
                };
            })
            .catch((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            });
        })


    @Effect()
    updateAppUser = this.actions$
        .ofType(AccountActions.UPDATE_APPUSER)
        .map((action: AccountActions.UpdateAppUser) => {
            return action.payload
        })
        .switchMap((appUser) => {
            return this.httpClient.put(this.baseUrl + 'appuser/' + appUser.id,
                appUser,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.alertify.success("更新しました");
                    return {
                        //type: AccountActions.SET_APPUSER, payload: appUser
                        type: AccountActions.GET_APPUSER, payload: appUser.id
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    updateMember = this.actions$
        .ofType(AccountActions.UPDATE_MEMBER)
        .map((action: AccountActions.UpdateMember) => {
            return action.payload
        })
        .switchMap((member) => {
            return this.httpClient.put(this.baseUrl + 'memberprofile/' + member.id,
                member,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.alertify.success("更新しました");
                    return {
                        type: AccountActions.GET_MEMBER_PROFILE, payload: member.id
                       // type: AccountActions.SET_MEMBER_PROFILE, payload: member
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    updateBisUser = this.actions$
        .ofType(AccountActions.UPDATE_BISUSER)
        .map((action: AccountActions.UpdateBisUser) => {
            return action.payload
        })
        .switchMap((bisuser) => {
            return this.httpClient.put(this.baseUrl + 'businessprofile/' + bisuser.id,
                bisuser,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.alertify.success("更新しました");
                    return {
                        type: AccountActions.GET_BUSINESS_PROFILE, payload: bisuser.id
                        //type: AccountActions.SET_BUSINESS_PROFILE, payload: bisuser
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    authLogout = this.actions$
        .ofType(AccountActions.LOGOUT)
        .mergeMap(() => {
            console.log("なにごと");
            this.router.navigate(['/home'])

            return [
                { type: GlobalActions.SUCCESS, payload: "ログアウトしました" },
                { type: PhotoActions.CLEAR_PHOTO},
                { type: BlogActions.CLEAR_BLOG}
            ]
        })

    @Effect()
    tokenExpired = this.actions$
        .ofType(AccountActions.TOKEN_EXPIRED)
        .mergeMap(() => {
            this.router.navigate(['/home'])
            return [
                { type: PhotoActions.CLEAR_PHOTO},
                { type: BlogActions.CLEAR_BLOG}
            ]
        })
}