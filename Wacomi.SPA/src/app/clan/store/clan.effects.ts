
import {mergeMap, catchError, map, switchMap, withLatestFrom} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";

import * as ClanSeekActions from "./clan.actions";
import * as GlobalActions from "../../store/global.actions";
import { ClanSeek } from "../../_models/ClanSeek";
import { of } from "rxjs";

import { Store } from "@ngrx/store";
import * as fromClan from "./clan.reducers";
import { ModalService } from "../../_services/modal.service";

@Injectable()
export class ClanSeekEffects {
    baseUrl = environment.apiUrl;
    readonly CLANSEEK_MAX = 5;
    constructor(private actions$: Actions,
        private store$: Store<fromClan.FeatureState>,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService,
        private modal: ModalService) { }

    @Effect()
    setClanSeekFilters = this.actions$
        .ofType(ClanSeekActions.SET_CLANSEEK_FILTERS).pipe(
        map(() => {
            return {
                type: ClanSeekActions.SEARCH_CLANSEEKS,
            }
        }))

    @Effect()
    setClanSeekPagination = this.actions$
        .ofType(ClanSeekActions.SET_CLANSEEK_PAGE).pipe(
        map(() => {
            return {
                type: ClanSeekActions.SEARCH_CLANSEEKS,
            }
        }))

    @Effect()
    searchClanSeeks = this.actions$
        .ofType(ClanSeekActions.SEARCH_CLANSEEKS).pipe(
        withLatestFrom(this.store$),
        switchMap(([actions, clanState]) => {
            let Params = new HttpParams();
            if (clanState.clan.selectedCategoryId > 0)
                Params = Params.append('categoryId', clanState.clan.selectedCategoryId.toString());
            if (clanState.clan.selectedCityId > 0)
                Params = Params.append('cityId', clanState.clan.selectedCityId.toString());
            if (clanState.clan.pagination) {
                Params = Params.append('pageNumber', clanState.clan.pagination.currentPage.toString());
                Params = Params.append('pageSize', clanState.clan.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek', { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: ClanSeekActions.SET_CLANSEEK_SEARCH_RESULT,
                        payload: {
                            clanSeeks: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),)

    @Effect()
    tryDeleteClanSeek = this.actions$
        .ofType(ClanSeekActions.TRY_DELETE_CLANSEEK).pipe(
        switchMap((actions: ClanSeekActions.TryDeleteClanSeek) => {
            return this.httpClient.delete(this.baseUrl + 'clanseek/' + actions.payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((result) => {
                    this.router.navigate(['/clan']);
                    return [{
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    },
                    {
                        type: ClanSeekActions.SET_COUNTLIMIT_FLAG, payload: false
                    }];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    tryAddClanSeek = this.actions$
        .ofType(ClanSeekActions.TRY_ADD_CLANSEEK).pipe(
        map((action: ClanSeekActions.TryAddClanSeek) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<ClanSeek>(this.baseUrl + 'clanseek',
                payload.clanSeek,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((result) => {
                    let returnValues: Array<any> = [
                        {
                            type: ClanSeekActions.SET_EDITING_CLANSEEK, payload: result
                        },
                        {
                            type: ClanSeekActions.CHECK_CLANSEEKS_COUNTLIMIT, payload: payload.clanSeek.appUserId
                        }
                    ];
                    if (payload.formData == null) {
                        this.alertify.success("投稿しました");
                        this.router.navigate(['/users/posts/' + payload.clanSeek.appUserId]);
                    }
                    else{
                        //returnValues.push({ type: ClanSeekActions.TRY_ADD_CLANSEEK_PHOTOS, payload: { clanSeekId: result.id, formData: payload.formData } });
                        returnValues.push({ type: GlobalActions.TRY_ADD_PHOTOS, payload: { recordType:"clanseek", recordId: result.id, formData:payload.formData, callbackLocation:'/users/posts/' + payload.clanSeek.appUserId } });
                    }

                    return returnValues;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    // @Effect()
    // tryAddClanSeekPhotos = this.actions$
    //     .ofType(ClanSeekActions.TRY_ADD_CLANSEEK_PHOTOS)
    //     .map((action: ClanSeekActions.TryAddClanSeekPhotos) => {
    //         return action.payload
    //     })
    //     .switchMap((payload) => {
    //         //this.alertify.success("写真をアップロード中…");
    //         this.modal.open(UploadingComponent);
    //         return this.httpClient.post(this.baseUrl + 'photo/clanseek/' + payload.clanSeekId,
    //             payload.formData)
    //             .mergeMap((result) => {
    //                 this.alertify.success("投稿しました");
    //                 this.modal.close();
    //                 this.router.navigate(['/clan']);
    //                 return [
    //                     {
    //                         type: ClanSeekActions.GET_CLANSEEK, payload: payload.clanSeekId
    //                     }
    //                 ];
    //             })
    //             .catch((error: string) => {
    //                 this.modal.close();
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

    @Effect()
    updateClanSeek = this.actions$
        .ofType(ClanSeekActions.UPDATE_CLANSEEK).pipe(
        map((action: ClanSeekActions.UpdateClanSeek) => {
            return action.payload
        }),
        switchMap((clanSeek) => {
            return this.httpClient.put(this.baseUrl + 'clanseek',
                clanSeek,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    this.alertify.success("更新しました");
                    this.router.navigate(['/users/posts', clanSeek.appUserId]);
                    return {
                        type: ClanSeekActions.SET_EDITING_CLANSEEK, payload: clanSeek
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    checkClanseeksCountLimit = this.actions$
        .ofType(ClanSeekActions.CHECK_CLANSEEKS_COUNTLIMIT).pipe(
        map((action: ClanSeekActions.CheckClanseeksCountLimit) => {
            return action.payload;
        }),
        switchMap((appUserId) => {
            return this.httpClient.get<number>(this.baseUrl + 'clanseek/user/' + appUserId + '/count').pipe(
                map((count) => {
                    return { type: ClanSeekActions.SET_COUNTLIMIT_FLAG, payload: count >= this.CLANSEEK_MAX }
                }
                ),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
    @Effect()
    getClanSeek = this.actions$
        .ofType(ClanSeekActions.GET_CLANSEEK).pipe(
        map((action: ClanSeekActions.GetClanSeek) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<ClanSeek>(this.baseUrl + 'clanseek/' + id).pipe(
                map((result) => {
                    return {
                        type: ClanSeekActions.SET_EDITING_CLANSEEK,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                }),);
        }),)

}