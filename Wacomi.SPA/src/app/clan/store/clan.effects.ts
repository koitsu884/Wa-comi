import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";

import * as ClanSeekActions from "./clan.actions";
import * as GlobalActions from "../../store/global.actions";
import { ClanSeek } from "../../_models/ClanSeek";
import { of } from "rxjs/observable/of";

@Injectable()
export class ClanSeekEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    @Effect()
    tryDeleteClanSeek = this.actions$
        .ofType(ClanSeekActions.TRY_DELETE_CLANSEEK)
        .switchMap((actions: ClanSeekActions.TryDeleteClanSeek) => {
            return this.httpClient.delete(this.baseUrl + 'clanseek/' + actions.payload,
            {
                headers: new HttpHeaders().set('Content-Type', 'application/json')
            })
            .map((result) => {
                this.router.navigate(['/clan']);
                return {
                    type: GlobalActions.SUCCESS, payload: "削除しました"
                };
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
                .map((result) => {
                    this.alertify.success("投稿しました");
                    return {
                        type: ClanSeekActions.SET_CLANSEEK, payload: result
                    };
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
                        return {
                            type: ClanSeekActions.SET_CLANSEEK, payload: clanSeek
                        };
                    })
                    .catch((error: string) => {
                        return of({ type: GlobalActions.FAILED, payload: error })
                    })
            });

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
                    type: ClanSeekActions.SET_CLANSEEK,
                    payload: result
                }
            })
            .catch((error: string) => {
                return of({ type: 'FAILED', payload: error })
            });
        })

}