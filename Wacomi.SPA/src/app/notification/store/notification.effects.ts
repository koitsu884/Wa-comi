
import {catchError, switchMap, map} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import * as NotificationActions from "./notification.action";
import * as GlobalActions from "../../store/global.actions";
import { of } from "rxjs";
import { AppNotification } from "../../_models/Notification";

@Injectable()
export class NotificationEffect {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient) { }

    @Effect()
    getNotifications = this.actions$
        .ofType(NotificationActions.GET_NOTIFICATIONS).pipe(
        map((action: NotificationActions.GetNotifications) => {return action.payload}),
        switchMap((userId) => {
            return this.httpClient.get<AppNotification[]>(this.baseUrl + 'notification/' + userId).pipe(
                map((notifications) => {
                    return {
                        type: NotificationActions.SET_NOTIFICATIONS,
                        payload: notifications
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryDeleteNotification = this.actions$
        .ofType(NotificationActions.TRY_DELETE_NOTIFICATION).pipe(
        map((action: NotificationActions.TryDeleteNotification) => {return action.payload}),
        switchMap((id) => {
            return this.httpClient.delete(this.baseUrl + 'notification/' + id).pipe(
                map(() => {
                    return {
                        type: NotificationActions.DELETE_NOTIFICATION,
                        payload: id
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}