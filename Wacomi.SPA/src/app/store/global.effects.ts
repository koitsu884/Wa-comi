import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../environments/environment";
import { Router } from "@angular/router";
import * as GlobalActions from "./global.actions";
import { City } from "../_models/City";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { of } from "rxjs/observable/of";
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
    getPropertyCategoryList = this.actions$
        .ofType(GlobalActions.GET_PROPERTY_CATEGORY_LIST)
        .switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'property/categories')
                .map((result) => {
                    return {
                        type: GlobalActions.SET_PROPERTY_CATEGORY_LIST,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddPhotos = this.actions$
        .ofType(GlobalActions.TRY_ADD_PHOTOS)
        .map((action: GlobalActions.TryAddPhotos) => {
            return action.payload
        })
        .switchMap((payload) => {
            this.modal.open(UploadingComponent);
            //return this.httpClient.post(this.baseUrl + 'photo/' + payload.recordType + '/' + payload.recordId,
            return this.httpClient.post(this.baseUrl + payload.recordType + '/' +  payload.recordId + '/photo',
                payload.formData)
                .map(() => {
                    this.modal.close();
                    this.router.navigate([payload.callbackLocation]);
                    return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
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

    @Effect()
    updateRecord = this.actions$
        .ofType(GlobalActions.UPDATE_RECORD)
        .map((action: GlobalActions.UpdateRecord) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + payload.recordType,
                payload.record,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap(() => {
                    let returnValues: Array<any> = [{ type: GlobalActions.SUCCESS, payload: "更新しました" }];
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);
                    if(payload.recordSetActionType)
                        returnValues.push({type: payload.recordSetActionType, payload: payload.record});

                    return returnValues;
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        });

    @Effect()
    tryDeleteRecord = this.actions$
        .ofType(GlobalActions.DELETE_RECORD)
        .map((actions: GlobalActions.DeleteRecord) => { return actions.payload })
        .switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + payload.recordType + '/' + payload.recordId,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.router.navigate([payload.callbackLocation]);
                    return {
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })
}