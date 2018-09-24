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
import { of } from 'rxjs/observable/of';

@Injectable()
export class CircleMemberEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromCircle.FeatureState>,
        private httpClient: HttpClient) { }

    @Effect()
    getLatestCircleMemberList = this.actions$
        .ofType(CircleMemberActions.GET_LATEST_CIRCLE_MEMBER_LIST)
        .map((action: CircleMemberActions.GetLatestCircleMemberList) => {
            return action.payload;
        })
        .switchMap((id) => {
            return this.httpClient.get<CircleMember[]>(this.baseUrl + 'circlemember/' + id + '/latest')
                .map((result) => {
                    return {
                        type: CircleMemberActions.SET_LATEST_CIRCLE_MEMBER_LIST,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getLatestCircleMemberMemberList = this.actions$
        .ofType(CircleMemberActions.GET_CIRCLE_MEMBER_LIST)
        .map((action: CircleMemberActions.GetCircleMemberList) => {
            return action.payload;
        })
        .withLatestFrom(this.store$)
        .switchMap(([payload, circleMemberState]) => {
            let Params = new HttpParams();
            if (!payload.initPage && circleMemberState.circleModule.circleMember.pagination) {
                Params = Params.append('pageNumber', circleMemberState.circleModule.circleMember.pagination.currentPage.toString());
                Params = Params.append('pageSize', circleMemberState.circleModule.circleMember.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<CircleMember[]>(this.baseUrl + 'circlemember/' + payload.circleId, { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: CircleMemberActions.SET_CIRCLE_MEMBER_LIST,
                        payload: {
                            memberList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddRecord = this.actions$
        .ofType(CircleMemberActions.SEND_CIRCLE_REQUEST)
        .map((action: CircleMemberActions.SendCircleRequest) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + 'circlemember/request',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap(() => {
                    return [
                        { type: GlobalActions.SUCCESS, payload: payload.requireApproval ? "リクエストを送りました" : "グループに加入しました" },
                        { type: CircleActions.GET_CIRCLE, payload: payload.circleId }
                    ];

                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    setCircleMemberPagination = this.actions$
        .ofType(CircleMemberActions.SET_CIRCLE_MEMBER_PAGE)
        .withLatestFrom(this.store$)
        .map(([action, circleMemberState]) => {
            return {
                type: CircleMemberActions.GET_CIRCLE_MEMBER_LIST,
                payload: { circleId: circleMemberState.circleModule.circle.selectedCircle.id }
            }
        })

    @Effect()
    deleteCircleMember = this.actions$
        .ofType(CircleMemberActions.DELETE_CIRCLE_MEMBER)
        .map((action: CircleMemberActions.DeleteCircleMember) => {
            return action.payload
        })
        .switchMap((circleMember) => {
            return this.httpClient.delete(this.baseUrl + 'circlemember/' + circleMember.circleId + '/' + circleMember.appUserId)
                .map(() => {
                    return {
                        type: GlobalActions.SUCCESS, payload: null
                    };
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                })
        });

}