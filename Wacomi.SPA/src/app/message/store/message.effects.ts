import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";

import * as MessageActions from "./message.actions";
import * as GlobalActions from "../../store/global.actions";
import { Message } from "../../_models/Message";
import { of } from "rxjs/observable/of";
import { Location } from "@angular/common";
import { Router } from "@angular/router";

@Injectable()
export class MessageEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient,
        private router: Router) { }

    @Effect()
    getMessageReceived = this.actions$
        .ofType(MessageActions.GET_MESSAGES_RECEIVED)
        .map((action: MessageActions.GetMessagesReceived) => {
            return action.payload;
        })
        .switchMap((appUserId) => {
            return this.httpClient.get<Message[]>(this.baseUrl + 'message/' + appUserId + '/received',
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map((result) => {
                    return {
                        type: MessageActions.SET_MESSAGES_RECEIVED, payload: result
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getMessageSent = this.actions$
        .ofType(MessageActions.GET_MESSAGES_SENT)
        .map((action: MessageActions.GetMessagesSent) => {
            return action.payload;
        })
        .switchMap((appUserId) => {
            return this.httpClient.get<Message[]>(this.baseUrl + 'message/' + appUserId + '/sent',
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map((result) => {
                    return {
                        type: MessageActions.SET_MESSAGES_SENT, payload: result
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    sendMessage = this.actions$
        .ofType(MessageActions.SEND_MESSAGE)
        .map((action: MessageActions.SendMessage) => {
            return action.payload;
        })
        .switchMap((newMessage) => {
            return this.httpClient.post<Message>(this.baseUrl + 'message',
                newMessage,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap((result) => {
                    this.router.navigate(['/message/', newMessage.senderId, 'sent']);
                    return [
                        // {
                        //     type: MessageActions.GET_MESSAGES_SENT, payload: newMessage.senderId
                        // },
                        { 
                            type: GlobalActions.SUCCESS, payload: "メッセージを送信しました" 
                        }
                    ];
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })
}