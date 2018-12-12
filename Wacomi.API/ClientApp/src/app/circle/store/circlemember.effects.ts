
import {mergeMap, catchError, switchMap, withLatestFrom, map} from 'rxjs/operators';
import * as fromCircle from '../store/circle.reducers';
import * as CircleActions from '../store/circle.actions';
import * as CircleMemberActions from '../store/circlemember.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CircleMember } from '../../_models/CircleMember';
import { of } from 'rxjs';

@Injectable()
export class CircleMemberEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromCircle.FeatureState>,
        private httpClient: HttpClient) { }



    @Effect()
    getLatestCircleMemberMemberList = this.actions$
        .ofType(CircleMemberActions.GET_CIRCLE_MEMBER_LIST).pipe(
        map((action: CircleMemberActions.GetCircleMemberList) => {
            return action.payload;
        }),
        withLatestFrom(this.store$),
        switchMap(([payload, circleMemberState]) => {
            let Params = new HttpParams();
            if (!payload.initPage && circleMemberState.circleModule.circleMember.pagination) {
                Params = Params.append('pageNumber', circleMemberState.circleModule.circleMember.pagination.currentPage.toString());
                Params = Params.append('pageSize', circleMemberState.circleModule.circleMember.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<CircleMember[]>(this.baseUrl + 'circlemember/' + payload.circleId, { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: CircleMemberActions.SET_CIRCLE_MEMBER_LIST,
                        payload: {
                            memberList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryAddRecord = this.actions$
        .ofType(CircleMemberActions.SEND_CIRCLE_REQUEST).pipe(
        map((action: CircleMemberActions.SendCircleRequest) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + 'circlemember/request',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
                    return [
                        { type: GlobalActions.SUCCESS, payload: payload.requireApproval ? "リクエストを送りました" : "グループに加入しました" },
                        { type: CircleActions.GET_CIRCLE, payload: payload.circleId }
                    ];

                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    setCircleMemberPagination = this.actions$
        .ofType(CircleMemberActions.SET_CIRCLE_MEMBER_PAGE).pipe(
        withLatestFrom(this.store$),
        map(([action, circleMemberState]) => {
            return {
                type: CircleMemberActions.GET_CIRCLE_MEMBER_LIST,
                payload: { circleId: circleMemberState.circleModule.circle.selectedCircle.id }
            }
        }),)

    @Effect()
    deleteCircleMember = this.actions$
        .ofType(CircleMemberActions.DELETE_CIRCLE_MEMBER).pipe(
        map((action: CircleMemberActions.DeleteCircleMember) => {
            return action.payload
        }),
        switchMap((circleMember) => {
            return this.httpClient.delete(this.baseUrl + 'circlemember/' + circleMember.circleId + '/' + circleMember.appUserId).pipe(
                mergeMap(() => {
                    return [
                    {
                        type: GlobalActions.SUCCESS, payload: null
                    },
                    { 
                        type:CircleActions.GET_CIRCLE, payload:circleMember.circleId
                    }
                ];
                }),
                catchError((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                }),)
        }),);

}