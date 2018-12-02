
import {mergeMap, switchMap, tap, catchError, map} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../environments/environment";
import { Router } from "@angular/router";
import * as GlobalActions from "./global.actions";
import { City } from "../_models/City";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { of } from "rxjs";
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
        .ofType(GlobalActions.GET_CITY_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<City[]>(this.baseUrl + 'city').pipe(
                map((cityList) => {
                    return {
                        type: GlobalActions.SET_CITY_LIST,
                        payload: cityList
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getHometownList = this.actions$
        .ofType(GlobalActions.GET_HOMETOWN_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<KeyValue[]>(this.baseUrl + 'hometown').pipe(
                map((homeTonwList) => {
                    return {
                        type: GlobalActions.SET_HOMETOWN_LIST,
                        payload: homeTonwList
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getClanCategoryList = this.actions$
        .ofType(GlobalActions.GET_CLANCATEGORY_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<KeyValue[]>(this.baseUrl + 'clanseek/categories').pipe(
                map((result) => {
                    return {
                        type: GlobalActions.SET_CLANCATEGORY_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getAttractionCategoryList = this.actions$
        .ofType(GlobalActions.GET_ATTRACTION_CATEGORY_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'attraction/categories').pipe(
                map((result) => {
                    return {
                        type: GlobalActions.SET_ATTRACTION_CATEGORY_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getPropertyCategoryList = this.actions$
        .ofType(GlobalActions.GET_PROPERTY_CATEGORY_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'property/categories').pipe(
                map((result) => {
                    return {
                        type: GlobalActions.SET_PROPERTY_CATEGORY_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getCircleCategoryList = this.actions$
        .ofType(GlobalActions.GET_CIRCLE_CATEGORY_LIST).pipe(
        switchMap(() => {
            return this.httpClient.get<Category[]>(this.baseUrl + 'circle/categories').pipe(
                map((result) => {
                    return {
                        type: GlobalActions.SET_CIRCLE_CATEGORY_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    tryAddPhotos = this.actions$
        .ofType(GlobalActions.TRY_ADD_PHOTOS).pipe(
        map((action: GlobalActions.TryAddPhotos) => {
            return action.payload
        }),
        switchMap((payload) => {
            this.modal.open(UploadingComponent);
            //return this.httpClient.post(this.baseUrl + 'photo/' + payload.recordType + '/' + payload.recordId,
            return this.httpClient.post(this.baseUrl + payload.recordType + '/' + payload.recordId + '/photo',
                payload.formData).pipe(
                mergeMap(() => {
                    var returnActions = payload.callbackActions ? payload.callbackActions : [];
                    this.modal.close();
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);

                    if(returnActions.length == 0)
                        returnActions.push({ type: GlobalActions.SUCCESS, payload: "投稿しました" });
                    return returnActions;
                }),
                catchError((error: string) => {
                    console.log(error);
                    this.modal.close();
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect({ dispatch: false })
    actionFailed = this.actions$
        .ofType(GlobalActions.FAILED).pipe(
        tap((action: GlobalActions.Failed) => {
            this.alertify.error(action.payload);
        }))

    @Effect({ dispatch: false })
    actionSuccess = this.actions$
        .ofType(GlobalActions.SUCCESS).pipe(
        tap((action: GlobalActions.Success) => {
            if (action.payload) {
                this.alertify.success(action.payload);
            }
        }))

    @Effect()
    getRecord = this.actions$
        .ofType(GlobalActions.GET_RECORD).pipe(
        map((action: GlobalActions.GetRecord) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.get<any>(this.baseUrl + payload.recordType + '/' + payload.recordId).pipe(
                map((result) => {
                    return {
                        type: payload.callbackAction,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryAddRecord = this.actions$
        .ofType(GlobalActions.TRY_ADD_RECORD).pipe(
        map((action: GlobalActions.TryAddRecord) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + payload.recordType,
                payload.record,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((result) => {
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
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    updateRecord = this.actions$
        .ofType(GlobalActions.UPDATE_RECORD).pipe(
        map((action: GlobalActions.UpdateRecord) => {
            return action.payload
        }),
        switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + payload.recordType,
                payload.record,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
                    let returnValues: Array<any> = [{ type: GlobalActions.SUCCESS, payload: "更新しました" }];
                    if (payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);
                    if (payload.recordSetActionType)
                        returnValues.push({ type: payload.recordSetActionType, payload: payload.record });

                    return returnValues;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    tryDeleteRecord = this.actions$
        .ofType(GlobalActions.DELETE_RECORD).pipe(
        map((actions: GlobalActions.DeleteRecord) => { return actions.payload }),
        switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + payload.recordType + '/' + payload.recordId,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
                    var returnActions = payload.callbackActions ? payload.callbackActions : [];
                    if(payload.callbackLocation)
                        this.router.navigate([payload.callbackLocation]);

                    returnActions.push({
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    });
                    return returnActions;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}