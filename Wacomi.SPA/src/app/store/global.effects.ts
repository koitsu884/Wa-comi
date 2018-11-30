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
    getCircleCategoryList = this.actions$
        .ofType(GlobalActions.GET_CIRCLE_CATEGORY_LIST)
        .switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'circle/categories')
                .map((result) => {
                    return {
                        type: GlobalActions.SET_CIRCLE_CATEGORY_LIST,
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
            return this.httpClient.post(this.baseUrl + payload.recordType + '/' + payload.recordId + '/photo',
                payload.formData)
                .mergeMap(() => {
                    var returnActions = payload.callbackActions ? payload.callbackActions : [];
                    this.modal.close();
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);

                    if(returnActions.length == 0)
                        returnActions.push({ type: GlobalActions.SUCCESS, payload: "投稿しました" });
                    return returnActions;
                })
                .catch((error: string) => {
                    console.log(error);
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
    getRecord = this.actions$
        .ofType(GlobalActions.GET_RECORD)
        .map((action: GlobalActions.GetRecord) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.get<any>(this.baseUrl + payload.recordType + '/' + payload.recordId)
                .map((result) => {
                    return {
                        type: payload.callbackAction,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddRecord = this.actions$
        .ofType(GlobalActions.TRY_ADD_RECORD)
        .map((action: GlobalActions.TryAddRecord) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + payload.recordType,
                payload.record,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap((result) => {
                    if(payload.formData){
                        return [{
                            type: GlobalActions.TRY_ADD_PHOTOS,
                            payload: {
                                recordType: payload.recordType,
                                recordId: result.id,
                                formData: payload.formData,
                                callbackLocation: payload.callbackLocation,
                                callbackActions: payload.callbackActions
                            }}];
                    }

                    var returnActions = payload.callbackActions ? payload.callbackActions : <{type:string, payload:any}[]>{};
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);
                    // return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
                    if(returnActions.length == 0)
                        returnActions.push({ type: GlobalActions.SUCCESS, payload: "投稿しました" });
                    
                    return returnActions;
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
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
                    if (payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);
                    if (payload.recordSetActionType)
                        returnValues.push({ type: payload.recordSetActionType, payload: payload.record });

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
                .mergeMap(() => {
                    var returnActions = payload.callbackActions ? payload.callbackActions : [];
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);

                    returnActions.push({
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    });
                    return returnActions;
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })
}