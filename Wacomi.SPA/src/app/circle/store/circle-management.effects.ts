
import {catchError, switchMap, map} from 'rxjs/operators';
import * as CircleManagementActions from './circle-management.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Circle } from '../../_models/Circle';
import { of } from 'rxjs';

@Injectable()
export class CircleManagementEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient) { }

    @Effect()
    getMyCircleList = this.actions$
        .ofType(CircleManagementActions.GET_MY_CIRCLE_LIST).pipe(
        map((action: CircleManagementActions.GetMyCircleList) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let Params = new HttpParams();
            if (payload.page) {
                Params = Params.append('pageNumber', payload.page.toString());
            }

            return this.httpClient.get<Circle[]>(this.baseUrl + 'circle/user/' + payload.userId, { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: CircleManagementActions.SET_MY_CIRCLE_LIST,
                        payload: {
                            circles: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),)

    @Effect()
    getOwnCircleList = this.actions$
        .ofType(CircleManagementActions.GET_OWN_CIRCLE_LIST).pipe(
        map((action: CircleManagementActions.GetOwnCircleList) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let Params = new HttpParams();
            if (payload.page) {
                Params = Params.append('pageNumber', payload.page.toString());
            }

            return this.httpClient.get<Circle[]>(this.baseUrl + 'circle/user/' + payload.userId + '/own', { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: CircleManagementActions.SET_OWN_CIRCLE_LIST,
                        payload: {
                            circles: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),)

    @Effect()
    setMyCirclePagination = this.actions$
        .ofType(CircleManagementActions.SET_MY_CIRCLE_PAGE).pipe(
        map((action: CircleManagementActions.SetMyCirclePage) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return of({
                type: CircleManagementActions.GET_MY_CIRCLE_LIST, payload: { userId: payload.userId, page: payload.page }
            });
        }),)

    @Effect()
    setOwnCirclePagination = this.actions$
        .ofType(CircleManagementActions.SET_OWN_CIRCLE_PAGE).pipe(
        map((action: CircleManagementActions.SetOwnCirclePage) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return of({
                type: CircleManagementActions.GET_OWN_CIRCLE_LIST, payload: { userId: payload.userId, page: payload.page }
            });
        }),)
}