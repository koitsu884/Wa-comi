import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../environments/environment";
import { Router } from "@angular/router";
import * as GlobalActions from "./global.actions";
import { City } from "../_models/City";
import { HttpClient } from "@angular/common/http";
import { of } from "rxjs/observable/of";
import { Hometown } from "../_models/Hometown";
import { AlertifyService } from "../_services/alertify.service";
import { KeyValue } from "../_models/KeyValue";

@Injectable()
export class GlobalEffect {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private alertify: AlertifyService,
        private httpClient: HttpClient) { }

    @Effect()
    getCityList = this.actions$
        .ofType(GlobalActions.GET_CITY_LIST)
        .switchMap(() => {
            return this.httpClient.get<City[]>(this.baseUrl + 'city')
                .map((cityList) => {
                    return {
                        type: GlobalActions.SET_CITY_LIST,
                        payload: cityList
                    }
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                });
        })

    @Effect()
    getHometownList = this.actions$
        .ofType(GlobalActions.GET_HOMETOWN_LIST)
        .switchMap(() => {
            return this.httpClient.get<KeyValue[]>(this.baseUrl + 'hometown')
                .map((homeTonwList) => {
                    return {
                        type: GlobalActions.SET_HOMETOWN_LIST,
                        payload: homeTonwList
                    }
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                });
        })

        @Effect()
        getClanCategoryList = this.actions$
            .ofType(GlobalActions.GET_CLANCATEGORY_LIST)
            .switchMap(() => {
                return this.httpClient.get<KeyValue[]>(this.baseUrl + 'clanseek/categories')
                    .map((result) => {
                        return {
                            type: GlobalActions.SET_CLANCATEGORY_LIST,
                            payload: result
                        }
                    })
                    .catch((error: string) => {
                        return of({ type: 'FAILED', payload: error })
                    });
            })

            @Effect({ dispatch: false })
            actionFailed = this.actions$
                .ofType(GlobalActions.FAILED)
                .do((action: GlobalActions.Failed) => {
                    this.alertify.error(action.payload);
                })
        
            @Effect({ dispatch: false })
            actionSuccess = this.actions$
                .ofType(GlobalActions.SUCCESS)
                .do((action: GlobalActions.Success) => {
                    if (action.payload) {
                        this.alertify.success(action.payload);
                    }
                })
}