
import {switchMap, map, mergeMap, catchError} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

import * as MessageActions from "./message.actions";
import * as GlobalActions from "../../store/global.actions";
import * as AccountAction from "../../account/store/account.actions";
import { Message } from "../../_models/Message";
import { of } from "rxjs";
import { Location } from "@angular/common";
import { Router } from "@angular/router";

@Injectable()
export class MessageEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient,
        private router: Router) { }

    @Effect()
    getMessages = this.actions$
        .ofType(MessageActions.GET_MESSAGES).pipe(
        map((action: MessageActions.GetMessages) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let httpParams = new HttpParams();
            if (payload.pageNumber > 0)
                httpParams = httpParams.append('pageNumber', payload.pageNumber.toString());
            if (payload.pageSize > 0)
                httpParams = httpParams.append('pageSize', payload.pageSize.toString());

            return this.httpClient.get<Message[]>(this.baseUrl + 'message/' + payload.appUserId + '/' + payload.type,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json'),
                    params: httpParams,
                    observe: 'response'
                }).pipe(
                map((response) => {
                    return {
                        type: MessageActions.SET_MESSAGES, payload: { messages: response.body, pagination: JSON.parse(response.headers.get("Pagination")) }
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    // @Effect()
    // getMessageReceived = this.actions$
    //     .ofType(MessageActions.GET_MESSAGES_RECEIVED)
    //     .map((action: MessageActions.GetMessagesReceived) => {
    //         return action.payload;
    //     })
    //     .switchMap((payload) => {
    //         let httpParams = new HttpParams();
    //         if(payload.pageNumber > 0)
    //             httpParams = httpParams.append('pageNumber', payload.pageNumber.toString());
    //         if(payload.pageSize > 0)
    //             httpParams = httpParams.append('pageSize', payload.pageSize.toString());

    //         return this.httpClient.get<Message[]>(this.baseUrl + 'message/' + payload.appUserId + '/received',
    //             {
    //                 headers: new HttpHeaders().set('Content-Type', 'application/json'),
    //                 params: httpParams,
    //                 observe: 'response'
    //             })
    //             .map((response) => {
    //                 return {
    //                     type: MessageActions.SET_MESSAGES_RECEIVED, payload: {messages: response.body, pagination: JSON.parse(response.headers.get("Pagination"))}
    //                 };
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

    // @Effect()
    // getMessageSent = this.actions$
    //     .ofType(MessageActions.GET_MESSAGES_SENT)
    //     .map((action: MessageActions.GetMessagesSent) => {
    //         return action.payload;
    //     })
    //     .switchMap((payload) => {
    //         let httpParams = new HttpParams();
    //         if(payload.pageNumber > 0)
    //             httpParams = httpParams.append('pageNumber', payload.pageNumber.toString());
    //         if(payload.pageSize > 0)
    //             httpParams = httpParams.append('pageSize', payload.pageSize.toString());

    //         return this.httpClient.get<Message[]>(this.baseUrl + 'message/' + payload.appUserId + '/sent',
    //             {
    //                 params: httpParams,
    //                 observe: 'response',
    //                 headers: new HttpHeaders().set('Content-Type', 'application/json')
    //             })
    //             .map((response) => {
    //                 return {
    //                     type: MessageActions.SET_MESSAGES_SENT, payload: {messages: response.body, pagination: JSON.parse(response.headers.get("Pagination"))}
    //                 };
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

    @Effect()
    sendMessage = this.actions$
        .ofType(MessageActions.SEND_MESSAGE).pipe(
        map((action: MessageActions.SendMessage) => {
            return action.payload;
        }),
        switchMap((newMessage) => {
            return this.httpClient.post<Message>(this.baseUrl + 'message',
                newMessage,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((result) => {
                    this.router.navigate(['/message/', newMessage.senderId, 'sent']);
                    return [
                        // {
                        //     type: MessageActions.GET_MESSAGES_SENT, payload: newMessage.senderId
                        // },
                        {
                            type: GlobalActions.SUCCESS, payload: "メッセージを送信しました"
                        }
                    ];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    setIsReadFlag = this.actions$
        .ofType(MessageActions.SET_ISREAD_FLAG).pipe(
        map((action: MessageActions.SetIsReadFlag) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.put(this.baseUrl + 'message/'+id+'/read',
                null,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((count:number) => {
                    return [
                        {
                            type: AccountAction.SET_NEWMESSAGES_COUNT, payload: count
                        }
                    ];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}