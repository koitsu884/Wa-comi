import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Actions, Effect } from '@ngrx/effects';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../_models/CircleEventParticipation';
import { of } from 'rxjs/observable/of';
import * as CircleEventParticipationsActions from '../store/circle-event-participations.actions';
import * as GlobalActions from '../../store/global.actions';

@Injectable()
export class CircleEventParticipationEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient) { }

    private getCircleEventParticipations(payload: any, setEventType: string, status: CircleEventParticipationStatus) {
        let Params = new HttpParams();
        if (payload.pageNumber)
            Params = Params.append('pageNumber', payload.pageNumber.toString());
        Params = Params.append('pageSize', '1');
        Params = Params.append('status', status.toString());

        return this.httpClient.get<CircleEventParticipation[]>(this.baseUrl + 'circleeventparticipation/' + payload.eventId, { params: Params, observe: 'response' })
            .map((response) => {
                return {
                    type: setEventType,
                    payload: {
                        participants: response.body,
                        pagination: JSON.parse(response.headers.get("Pagination"))
                    }
                }
            })
            .catch((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            });
    }

    @Effect()
    getCircleEventConfirmedList = this.actions$
        .ofType(CircleEventParticipationsActions.GET_CIRCLE_EVENT_CONFIRMEDLIST)
        .map((action: CircleEventParticipationsActions.GetCircleEventConfirmedList) => {
            return action.payload;
        })
        .switchMap((payload) => {
            let status = CircleEventParticipationStatus.Confirmed;
            let setEventType = CircleEventParticipationsActions.SET_CIRCLE_EVENT_CONFIRMEDLIST;
            return this.getCircleEventParticipations(payload, setEventType, status);
            // if(payload.pageNumber)
            //     Params = Params.append('pageNumber', payload.pageNumber.toString());
            // Params = Params.append('status', status.toString());

            // return this.httpClient.get<CircleEventParticipation[]>(this.baseUrl + 'circleeventparticipation/' + payload.eventId, { params: Params, observe: 'response' })
            //     .map((response) => {
            //         return {
            //             type: setEventType,
            //             payload: {
            //                 participants: response.body,
            //                 pagination: JSON.parse(response.headers.get("Pagination"))
            //             }
            //         }
            //     })
            //     .catch((error: string) => {
            //         return of({ type: GlobalActions.FAILED, payload: error })
            //     });
        })

    @Effect()
    getCircleEventWaitList = this.actions$
        .ofType(CircleEventParticipationsActions.GET_CIRCLE_EVENT_WAITLIST)
        .map((action: CircleEventParticipationsActions.GetCircleEventWaitList) => {
            return action.payload;
        })
        .switchMap((payload) => {
            let status = CircleEventParticipationStatus.Waiting;
            let setEventType = CircleEventParticipationsActions.SET_CIRCLE_EVENT_WAITLIST;
            return this.getCircleEventParticipations(payload, setEventType, status);
        })

    @Effect()
    getCircleEventCanceledList = this.actions$
        .ofType(CircleEventParticipationsActions.GET_CIRCLE_EVENT_CANCELEDLIST)
        .map((action: CircleEventParticipationsActions.GetCircleEventCanceledList) => {
            return action.payload;
        })
        .switchMap((payload) => {
            let status = CircleEventParticipationStatus.Canceled;
            let setEventType = CircleEventParticipationsActions.SET_CIRCLE_EVENT_CANCELEDLIST;
            return this.getCircleEventParticipations(payload, setEventType, status);
        })

    @Effect()
    approveCircleRequest = this.actions$
        .ofType(CircleEventParticipationsActions.APPROVE_EVENT_PARTICIPATION_REQUEST)
        .map((action: CircleEventParticipationsActions.ApproveEventParticipationRequest) => {
            return action.payload;
        })
        .switchMap((eventParticipation) => {
            return this.httpClient.put<CircleEventParticipation>(this.baseUrl + 'circleeventparticipation/approve',
                eventParticipation,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap(() => {
                    return [{
                        type: GlobalActions.SUCCESS,
                        payload: "参加を認証しました"
                    }];
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

}