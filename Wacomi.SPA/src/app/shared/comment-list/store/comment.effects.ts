
import {catchError, switchMap, map} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import * as CommentActions from './comment.actions';
import * as GlobalActions from '../../../store/global.actions';
import { HttpClient, HttpParams } from "@angular/common/http";
import { UserComment } from "../../../_models/UserComment";
import { of } from "rxjs";
import { ShortComment } from "../../../_models/ShortComment";


@Injectable()
export class CommentEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient) { }


    @Effect()
    getUserCommentList = this.actions$
        .ofType(CommentActions.GET_USER_COMMENT_LIST).pipe(
        map((action: CommentActions.GetUserCommentList) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let Params = new HttpParams();
            if (payload.pageNumber) {
                Params = Params.append('pageNumber', payload.pageNumber.toString());
             //   Params = Params.append('pageSize', payload.pagination.itemsPerPage.toString());
            }

            let recordType = payload.ownerRecordType.toLowerCase();
            let url = this.baseUrl;
            switch(recordType){
                default:
                    url += recordType + '/' + payload.ownerRecordId + '/comments';
                    break;
            }

            return this.httpClient.get<UserComment[]>(url, { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: CommentActions.SET_USER_COMMENT_LIST,
                        payload: {
                            commentList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getForcusedTopicComment = this.actions$
        .ofType(CommentActions.GET_FORCUSED_COMMENT).pipe(
        map((action: CommentActions.GetForcusedUserComment) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let recordType = payload.ownerRecordType.toLowerCase();
            let url = this.baseUrl;
            switch(recordType){
                default:
                    url += recordType + 'comment/' + payload.id;
                    break;
            }

            return this.httpClient.get<UserComment>(url).pipe(
                map((circleTopicComment) => {
                    return {
                        type: CommentActions.SET_USER_COMMENT_LIST,
                        payload: {
                            commentList: [circleTopicComment],
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getCircleTopicReplies = this.actions$
        .ofType(CommentActions.GET_USER_REPLIES).pipe(
        map((action: CommentActions.GetUserReplies) => {
            return action.payload;
        }),
        switchMap((payload) => {
            let recordType = payload.ownerRecordType.toLowerCase();
            let url = this.baseUrl;
            switch(recordType){
                default:
                    url += recordType + 'comment/' + payload.commentId + '/replies';
                    break;
            }

            return this.httpClient.get<ShortComment[]>(url).pipe(
                map((result) => {
                    return {
                        type: CommentActions.SET_USER_REPLIES,
                        payload: { ownerRecordType: payload.ownerRecordType, ownerRecordId: payload.commentId, userReplies: result }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
}