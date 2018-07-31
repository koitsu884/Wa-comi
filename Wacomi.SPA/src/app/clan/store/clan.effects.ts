import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";

import * as ClanSeekActions from "./clan.actions";
import * as GlobalActions from "../../store/global.actions";
import { ClanSeek } from "../../_models/ClanSeek";
import { of } from "rxjs/observable/of";
import 'rxjs/add/operator/withLatestFrom';
import { Store } from "@ngrx/store";
import * as fromClan from "./clan.reducers";

@Injectable()
export class ClanSeekEffects {
    baseUrl = environment.apiUrl;
    readonly CLANSEEK_MAX = 5;
    constructor(private actions$: Actions,
        private store$: Store<fromClan.FeatureState>,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    @Effect()
    setClanSeekFilters = this.actions$
        .ofType(ClanSeekActions.SET_CLANSEEK_FILTERS)
        .map(() => {
            return {
                type: ClanSeekActions.SEARCH_CLANSEEKS,
            }
        })

    @Effect()
    setClanSeekPagination = this.actions$
        .ofType(ClanSeekActions.SET_CLANSEEK_PAGE)
        .map(() => {
            return {
                type: ClanSeekActions.SEARCH_CLANSEEKS,
            }
        })

    @Effect()
    searchClanSeeks = this.actions$
        .ofType(ClanSeekActions.SEARCH_CLANSEEKS)
        .withLatestFrom(this.store$)
        .switchMap(([actions, clanState]) => {
            let Params = new HttpParams();
            if (clanState.clan.selectedCategoryId > 0)
                Params = Params.append('categoryId', clanState.clan.selectedCategoryId.toString());
            if (clanState.clan.selectedCityId > 0)
                Params = Params.append('cityId', clanState.clan.selectedCityId.toString());
            if (clanState.clan.pagination) {
                Params = Params.append('pageNumber', clanState.clan.pagination.currentPage.toString());
                Params = Params.append('pageSize', clanState.clan.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<ClanSeek[]>(this.baseUrl + 'clanseek', { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: ClanSeekActions.SET_CLANSEEK_SEARCH_RESULT,
                        payload: {
                            clanSeeks: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        })

    @Effect()
    tryDeleteClanSeek = this.actions$
        .ofType(ClanSeekActions.TRY_DELETE_CLANSEEK)
        .switchMap((actions: ClanSeekActions.TryDeleteClanSeek) => {
            return this.httpClient.delete(this.baseUrl + 'clanseek/' + actions.payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap((result) => {
                    this.router.navigate(['/clan']);
                    return [{
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    },
                    {
                        type: ClanSeekActions.SET_COUNTLIMIT_FLAG, payload: false
                    }];
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddClanSeek = this.actions$
        .ofType(ClanSeekActions.TRY_ADD_CLANSEEK)
        .map((action: ClanSeekActions.TryAddClanSeek) => {
            return action.payload;
        })
        .switchMap((clanSeek) => {
            return this.httpClient.post<ClanSeek>(this.baseUrl + 'clanseek',
                clanSeek,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap((result) => {
                    this.alertify.success("投稿しました");
                    this.router.navigate(['/clan']);
                    return [
                        {
                            type: ClanSeekActions.SET_EDITING_CLANSEEK, payload: result
                        },
                        {
                            type: ClanSeekActions.CHECK_CLANSEEKS_COUNTLIMIT, payload: clanSeek.appUserId
                        }
                    ];
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    updateClanSeek = this.actions$
        .ofType(ClanSeekActions.UPDATE_CLANSEEK)
        .map((action: ClanSeekActions.UpdateClanSeek) => {
            return action.payload
        })
        .switchMap((clanSeek) => {
            return this.httpClient.put(this.baseUrl + 'clanseek',
                clanSeek,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.alertify.success("更新しました");
                    this.router.navigate(['/clan']);
                    return {
                        type: ClanSeekActions.SET_EDITING_CLANSEEK, payload: clanSeek
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        });

    @Effect()
    checkClanseeksCountLimit = this.actions$
        .ofType(ClanSeekActions.CHECK_CLANSEEKS_COUNTLIMIT)
        .map((action: ClanSeekActions.CheckClanseeksCountLimit) => {
            return action.payload;
        })
        .switchMap((appUserId) => {
            return this.httpClient.get<number>(this.baseUrl + 'clanseek/user/' + appUserId + '/count')
                .map((count) => {
                    return { type: ClanSeekActions.SET_COUNTLIMIT_FLAG, payload: count >= this.CLANSEEK_MAX }
                }
                )
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })
    @Effect()
    getClanSeek = this.actions$
        .ofType(ClanSeekActions.GET_CLANSEEK)
        .map((action: ClanSeekActions.GetClanSeek) => {
            return action.payload;
        })
        .switchMap((id) => {
            return this.httpClient.get<ClanSeek>(this.baseUrl + 'clanseek/' + id)
                .map((result) => {
                    return {
                        type: ClanSeekActions.SET_EDITING_CLANSEEK,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                });
        })

}