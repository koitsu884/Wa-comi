
import {withLatestFrom, catchError, mergeMap, switchMap, map} from 'rxjs/operators';
import * as fromCircle from './circle.reducers';
import * as CircleActions from './circle.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Circle } from '../../_models/Circle';
import { of } from 'rxjs';

import { CircleRequest } from '../../_models/CircleRequest';
import { CircleTopic } from '../../_models/CircleTopic';
import { CircleMember } from '../../_models/CircleMember';
import { CircleEvent } from '../../_models/CircleEvent';

@Injectable()
export class CircleEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromCircle.FeatureState>,
        private router: Router,
        private httpClient: HttpClient) { }

    @Effect()
    getCircle = this.actions$
        .ofType(CircleActions.GET_CIRCLE).pipe(
        map((action: CircleActions.GetCircle) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<Circle>(this.baseUrl + 'circle/' + id).pipe(
                mergeMap((result) => {
                    var returnValue = [
                        {
                            type: CircleActions.SET_CIRCLE,
                            payload: result
                        },
                        {
                            type: CircleActions.GET_LATEST_CIRCLE_MEMBER_LIST,
                            payload: id
                        },
                        {
                            type: CircleActions.GET_LATEST_CIRCLE_TOPIC_LIST,
                            payload: id
                        },
                        {
                            type: CircleActions.GET_LATEST_CIRCLE_EVENT_LIST,
                            payload: id
                        },
                        // {
                        //     type: CircleMemberActions.GET_CIRCLE_MEMBER_LIST,
                        //     payload: {circleId : id, initPage: true}
                        // },
                        // {
                        //     type: CircleTopicActions.GET_CIRCLE_TOPIC_LIST,
                        //     payload: {circleId : id, initPage: true}
                        // }
                    ]

                    if (result.isManageable)
                        returnValue.push(
                            {
                                type: CircleActions.GET_CIRCLE_REQUEST_LIST,
                                payload: id
                            }
                        )
                    return returnValue;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)


    @Effect()
    setCircleFilters = this.actions$
        .ofType(CircleActions.SET_CIRCLE_SEARCH_OPTIONS).pipe(
        map(() => {
            return {
                type: CircleActions.SEARCH_CIRCLE,
            }
        }))

    @Effect()
    setCirclePagination = this.actions$
        .ofType(CircleActions.SET_CIRCLE_PAGE).pipe(
        map(() => {
            return {
                type: CircleActions.SEARCH_CIRCLE,
            }
        }))

    @Effect()
    searchCircle = this.actions$
        .ofType(CircleActions.SEARCH_CIRCLE).pipe(
        withLatestFrom(this.store$),
        switchMap(([actions, circleState]) => {
            let Params = new HttpParams();
            if (circleState.circleModule.circle.searchParam.categoryId > 0)
                Params = Params.append('categoryId', circleState.circleModule.circle.searchParam.categoryId.toString());
            if (circleState.circleModule.circle.searchParam.cityId > 0)
                Params = Params.append('cityId', circleState.circleModule.circle.searchParam.cityId.toString());
            if (circleState.circleModule.circle.pagination) {
                Params = Params.append('pageNumber', circleState.circleModule.circle.pagination.currentPage.toString());
                Params = Params.append('pageSize', circleState.circleModule.circle.pagination.itemsPerPage.toString());
            }

            return this.httpClient.post<Circle[]>(
                this.baseUrl + 'circle/search',
                circleState.circleModule.circle.searchParam,
                { params: Params, observe: 'response' }
            ).pipe(
                map((response) => {
                    return {
                        type: CircleActions.SET_CIRCLE_SEARCH_RESULT,
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
    getCircleRequestList = this.actions$
        .ofType(CircleActions.GET_CIRCLE_REQUEST_LIST).pipe(
        map((action: CircleActions.GetCircleRequestList) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleRequest[]>(this.baseUrl + 'circlemember/' + id + '/request').pipe(
                map((result) => {
                    return {
                        type: CircleActions.SET_CIRCLE_REQUEST_LIST,
                        payload: result
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    approveCircleRequest = this.actions$
        .ofType(CircleActions.APPROVE_CIRCLE_REQUEST).pipe(
        map((action: CircleActions.ApproveCircleRequest) => {
            return action.payload;
        }),
        switchMap((circleRequest) => {
            return this.httpClient.post<CircleMember>(this.baseUrl + 'circlemember/approve',
                circleRequest,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((newMember) => {
                    return [{
                        type: GlobalActions.SUCCESS,
                        payload: "メンバー認証しました"
                    },
                    {
                        type: CircleActions.ADD_NEW_CIRCLE_MEMBER,
                        payload: newMember
                    }];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getLatestCircleMemberList = this.actions$
        .ofType(CircleActions.GET_LATEST_CIRCLE_MEMBER_LIST).pipe(
        map((action: CircleActions.GetLatestCircleMemberList) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleMember[]>(this.baseUrl + 'circlemember/' + id + '/latest').pipe(
                map((result) => {
                    return {
                        type: CircleActions.SET_LATEST_CIRCLE_MEMBER_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getLatestCircleTopicList = this.actions$
        .ofType(CircleActions.GET_LATEST_CIRCLE_TOPIC_LIST).pipe(
        map((action: CircleActions.GetLatestCircleTopicList) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleTopic[]>(this.baseUrl + 'circle/' + id + '/topics/latest').pipe(
                map((result) => {
                    return {
                        type: CircleActions.SET_LATEST_CIRCLE_TOPIC_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getLatestCircleEventList = this.actions$
        .ofType(CircleActions.GET_LATEST_CIRCLE_EVENT_LIST).pipe(
        map((action: CircleActions.GetLatestCircleEventList) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleEvent[]>(this.baseUrl + 'circle/' + id + '/events/latest').pipe(
                map((result) => {
                    return {
                        type: CircleActions.SET_LATEST_CIRCLE_EVENT_LIST,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

        @Effect()
        getPastCircleEventList = this.actions$
            .ofType(CircleActions.GET_PAST_CIRCLE_EVENT_LIST).pipe(
            map((action: CircleActions.GetPastCircleEventList) => {
                return action.payload;
            }),
            switchMap((id) => {
                return this.httpClient.get<CircleEvent[]>(this.baseUrl + 'circle/' + id + '/events/past').pipe(
                    map((result) => {
                        return {
                            type: CircleActions.SET_PAST_CIRCLE_EVENT_LIST,
                            payload: result
                        }
                    }),
                    catchError((error: string) => {
                        return of({ type: GlobalActions.FAILED, payload: error })
                    }),);
            }),)

    @Effect()
    declineCircleRequest = this.actions$
        .ofType(CircleActions.DECLINE_CIRCLE_REQUEST).pipe(
        map((action: CircleActions.DeclineCircleRequest) => {
            return action.payload;
        }),
        switchMap((circleRequest) => {
            return this.httpClient.put<any>(this.baseUrl + 'circlemember/decline',
                circleRequest,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((result) => {
                    return {
                        type: GlobalActions.FAILED,
                        payload: "リクエストを拒否しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}