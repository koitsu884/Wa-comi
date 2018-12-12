
import {withLatestFrom, catchError, mergeMap, map, switchMap} from 'rxjs/operators';
import * as fromCircle from '../store/circle.reducers';
import * as CircleEventActions from '../store/circleevent.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CircleEvent } from '../../_models/CircleEvent';
import { of } from 'rxjs';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../_models/CircleEventParticipation';

@Injectable()
export class CircleEventEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromCircle.FeatureState>,
        private httpClient: HttpClient) { }

    @Effect()
    getCircleEvent = this.actions$
        .ofType(CircleEventActions.GET_CIRCLE_EVENT).pipe(
        map((action: CircleEventActions.GetCircleEvent) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleEvent>(this.baseUrl + 'circleevent/' + id).pipe(
                mergeMap((result) => {
                    var returnValue = [
                        {
                            type: CircleEventActions.SET_CIRCLE_EVENT,
                            payload: result
                        }
                    ]

                    return returnValue;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)


    @Effect()
    searchCircleEvent = this.actions$
        .ofType(CircleEventActions.SEARCH_CIRCLE_EVENT).pipe(
        map((action: CircleEventActions.SearchCircleEvent) => {
            return action.payload;
        }),
        withLatestFrom(this.store$),
        switchMap(([payload, circleState]) => {
            if ((payload.cityId == circleState.circleModule.circleEvent.cityId
                && payload.categoryId == circleState.circleModule.circleEvent.categoryId
                && payload.fromDate == circleState.circleModule.circleEvent.fromDate
                && payload.circleId == circleState.circleModule.circleEvent.circleId
            )
                && !circleState.circleModule.circleEvent.reload)
                return of({ type: GlobalActions.SUCCESS, payload: null });

            return of({ type: CircleEventActions.SET_CIRCLE_EVENT_SEARCH_PARAMS, payload: payload });
        }),)

    @Effect()
    setCircleEventFilters = this.actions$
        .ofType(CircleEventActions.SET_CIRCLE_EVENT_SEARCH_PARAMS).pipe(
        map(() => {
            return {
                type: CircleEventActions.GET_CIRCLE_EVENT_LIST, payload: 1
            }
        }))


    @Effect()
    loadNextCircleEventList = this.actions$
        .ofType(CircleEventActions.LOAD_NEXT_CIRCLE_EVENT_LIST).pipe(
        withLatestFrom(this.store$),
        map(([action, circleState]) => {
            if (circleState.circleModule.circleEvent.finish) {
                return {
                    type: GlobalActions.SUCCESS,
                    payload: null
                }
            }
            return {
                type: CircleEventActions.GET_CIRCLE_EVENT_LIST, payload: circleState.circleModule.circleEvent.lastPageNumber,
            }
        }),)

    @Effect()
    getCircleEventList = this.actions$
        .ofType(CircleEventActions.GET_CIRCLE_EVENT_LIST).pipe(
        map((action: CircleEventActions.GetCircleEventList) => {
            return action.payload;
        }),
        withLatestFrom(this.store$),
        switchMap(([pageNumber, circleState]) => {
            let Params = new HttpParams();
            if (circleState.circleModule.circleEvent.circleId > 0)
                Params = Params.append('circleId', circleState.circleModule.circleEvent.circleId.toString());
            if (circleState.circleModule.circleEvent.categoryId > 0)
                Params = Params.append('circleCategoryId', circleState.circleModule.circleEvent.categoryId.toString());
            if (circleState.circleModule.circleEvent.cityId > 0)
                Params = Params.append('cityId', circleState.circleModule.circleEvent.cityId.toString());
            if (circleState.circleModule.circleEvent.fromDate)
                Params = Params.append('fromDate', circleState.circleModule.circleEvent.fromDate.toString());

            Params = Params.append('pageNumber', pageNumber.toString());
            // Params = Params.append('pageSize', '2');//Test


            return this.httpClient.get<CircleEvent[]>(
                this.baseUrl + 'circleevent',
                { params: Params }
            ).pipe(
                map((circleEvents) => {
                    if (circleEvents.length == 0) {
                        return {
                            type: CircleEventActions.SET_EVENT_LOAD_FINISH_FLAG
                        }
                    }

                    return {
                        type: CircleEventActions.ADD_CIRCLE_EVENT_LIST,
                        payload: circleEvents
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),)

    @Effect()
    sendEventParticipationRequest = this.actions$
        .ofType(CircleEventActions.SEND_EVENT_PARTICIPATION_REQUEST).pipe(
        map((action: CircleEventActions.SendEventParticipationRequest) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<CircleEventParticipation>(this.baseUrl + 'circleeventparticipation',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((result) => {
                    var returnValue = [
                        { type: GlobalActions.SUCCESS, payload: "リクエストを送りました" },
                        { type: CircleEventActions.SET_EVENT_PARTICIPATION_STATUS, payload: result.status }
                    ];
                    if (result.status == CircleEventParticipationStatus.Confirmed)
                    {
                        returnValue.push({ type: CircleEventActions.GET_LATEST_EVENT_PARTICIPANTS, payload: payload.circleEventId });
                    }

                    return returnValue;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
    @Effect()
    getLatestEventParticipations = this.actions$
        .ofType(CircleEventActions.GET_LATEST_EVENT_PARTICIPANTS).pipe(
        map((action: CircleEventActions.GetLatestEventParticipants) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.get<CircleEventParticipation[]>(this.baseUrl + 'circleeventparticipation/' + payload + '/latest').pipe(
                map((result) => {
                    return {
                        type: CircleEventActions.SET_LATEST_EVENT_PARTICIPANTS,
                        payload: result,
                    }
                }
                ),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    deleteEventParticipation = this.actions$
        .ofType(CircleEventActions.DELETE_EVENT_PARTICIPATION).pipe(
        map((action: CircleEventActions.DeleteEventParticipation) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + 'circleeventparticipation/' + payload.appUserId + '/' + payload.circleEventId,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    return {
                        type: GlobalActions.SUCCESS,
                        payload: "リクエストをキャンセルしました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    cancelEventParticipation = this.actions$
        .ofType(CircleEventActions.CANCEL_EVENT_PARTICIPATION).pipe(
        map((action: CircleEventActions.CancelEventParticipation) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + 'circleeventparticipation/cancel',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
                    return [
                        {
                            type: CircleEventActions.GET_LATEST_EVENT_PARTICIPANTS,
                            payload: payload.circleEventId
                        },
                        {
                        type: GlobalActions.SUCCESS,
                        payload: "参加をキャンセルしました"
                    }];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}