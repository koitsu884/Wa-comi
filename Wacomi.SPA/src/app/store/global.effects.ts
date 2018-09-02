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
import { Category } from "../_models/Category";
import { ModalService } from "../_services/modal.service";
import { UploadingComponent } from "../core/modal/uploading/uploading.component";

@Injectable()
export class GlobalEffect {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private alertify: AlertifyService,
        private modal: ModalService,
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
                    return of({ type: GlobalActions.FAILED, payload: error })
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
                    return of({ type: GlobalActions.FAILED, payload: error })
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
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getAttractionCategoryList = this.actions$
        .ofType(GlobalActions.GET_ATTRACTION_CATEGORY_LIST)
        .switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'attraction/categories')
                .map((result) => {
                    return {
                        type: GlobalActions.SET_ATTRACTION_CATEGORY_LIST,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddClanSeekPhotos = this.actions$
        .ofType(GlobalActions.TRY_ADD_PHOTOS)
        .map((action: GlobalActions.TryAddPhotos) => {
            return action.payload
        })
        .switchMap((payload) => {
            this.modal.open(UploadingComponent);
            return this.httpClient.post(this.baseUrl + 'photo/' + payload.recordType + '/' + payload.recordId,
                payload.formData)
                .map(() => {
                    this.modal.close();
                    this.router.navigate(['/' + payload.recordType]);
                    return { type: GlobalActions.SUCCESS, payload: "投稿しました"};
                })
                .catch((error: string) => {
                    this.modal.close();
                    return of({ type: GlobalActions.FAILED, payload: error })
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