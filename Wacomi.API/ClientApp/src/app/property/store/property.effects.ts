
import {withLatestFrom, catchError, switchMap, map} from 'rxjs/operators';
import * as fromProperty from '../store/property.reducers';
import * as PropertyActions from '../store/property.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { of } from 'rxjs';
import { Property } from '../../_models/Property';

@Injectable()
export class PropertyEffects {
    baseUrl = environment.apiUrl;
    readonly PROPERTY_MAX = 3;
    constructor(private actions$: Actions,
        private store$: Store<fromProperty.FeatureState>,
        private router: Router,
        private httpClient: HttpClient) { }

    @Effect()
    tryAddProperty = this.actions$
        .ofType(PropertyActions.TRY_ADD_PROPERTY).pipe(
        map((action: PropertyActions.TryAddProperty) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<Property>(this.baseUrl + 'property',
                payload.property,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((result) => {
                    if (payload.formData == null) {
                        this.router.navigate(['/property']);
                        return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
                    }

                    return {
                        type: GlobalActions.TRY_ADD_PHOTOS,
                        payload: {
                            recordType: 'property',
                            recordId: result.id,
                            formData: payload.formData,
                            callbackLocation: '/users/posts/' + payload.property.appUserId
                        }
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    updateProperty = this.actions$
        .ofType(PropertyActions.UPDATE_PROPERTY).pipe(
        map((action: PropertyActions.UpdateProperty) => {
            return action.payload
        }),
        switchMap((property) => {
            return this.httpClient.put(this.baseUrl + 'property',
                property,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    this.router.navigate(['/users/posts', property.appUserId]);
                    return {
                        type: GlobalActions.SUCCESS, payload: "更新しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    setPropertySearchOptions = this.actions$
        .ofType(PropertyActions.SET_PROPERTY_SEARCH_OPTIONS).pipe(
        map(() => {
            return {
                type: PropertyActions.SEARCH_PROPERTIES,
            }
        }))

    @Effect()
    setClanSeekPagination = this.actions$
        .ofType(PropertyActions.SET_PROPERTY_PAGE).pipe(
        map(() => {
            return {
                type: PropertyActions.SEARCH_PROPERTIES,
            }
        }))

    @Effect()
    searchProperties = this.actions$
        .ofType(PropertyActions.SEARCH_PROPERTIES).pipe(
        withLatestFrom(this.store$),
        switchMap(([action, propertyState]) => {
            let Params = new HttpParams();
            if (propertyState.property.pagination) {
                Params = Params.append('pageNumber', propertyState.property.pagination.currentPage.toString());
                Params = Params.append('pageSize', propertyState.property.pagination.itemsPerPage.toString());
            }

            return this.httpClient.post<Property[]>(
                this.baseUrl + 'property/search',
                propertyState.property.searchParams,
                { params: Params, observe: 'response' }
            ).pipe(
                map((response) => {
                    return {
                        type: PropertyActions.SET_PROPERTY_SEARCH_RESULT,
                        payload: {
                            properties: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),)


    @Effect()
    getProperty = this.actions$
        .ofType(PropertyActions.GET_PROPERTY).pipe(
        map((action: PropertyActions.GetProperty) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<Property>(this.baseUrl + 'property/' + id).pipe(
                map((result) => {
                    return {
                        type: PropertyActions.SET_PROPERTY,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                }),);
        }),)

    @Effect()
    tryDeleteProperty = this.actions$
        .ofType(PropertyActions.TRY_DELETE_PROPERTY).pipe(
        map((actions: PropertyActions.TryDeleteProperty) => {
            return {
                type: GlobalActions.DELETE_RECORD, payload:{recordType:'property', recordId:actions.payload, callbackLocation:'/property'}
            }
        }))

}