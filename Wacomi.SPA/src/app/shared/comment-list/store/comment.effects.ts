import { Injectable } from "@angular/core";
import { environment } from "../../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import * as CommentActions from './comment.actions';
import * as GlobalActions from '../../../store/global.actions';
import { HttpClient, HttpParams } from "@angular/common/http";
import { UserComment } from "../../../_models/UserComment";
import { of } from "rxjs/observable/of";
import { ShortComment } from "../../../_models/ShortComment";


@Injectable()
export class CommentEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient) { }


    @Effect()
    getUserCommentList = this.actions$
        .ofType(CommentActions.GET_USER_COMMENT_LIST)
        .map((action: CommentActions.GetUserCommentList) => {
            return action.payload;
        })
        .switchMap((payload) => {
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

            return this.httpClient.get<UserComment[]>(url, { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: CommentActions.SET_USER_COMMENT_LIST,
                        payload: {
                            commentList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getForcusedTopicComment = this.actions$
        .ofType(CommentActions.GET_FORCUSED_COMMENT)
        .map((action: CommentActions.GetForcusedUserComment) => {
            return action.payload;
        })
        .switchMap((payload) => {
            let recordType = payload.ownerRecordType.toLowerCase();
            let url = this.baseUrl;
            switch(recordType){
                default:
                    url += recordType + 'comment/' + payload.id;
                    break;
            }

            return this.httpClient.get<UserComment>(url)
                .map((circleTopicComment) => {
                    return {
                        type: CommentActions.SET_USER_COMMENT_LIST,
                        payload: {
                            commentList: [circleTopicComment],
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getCircleTopicReplies = this.actions$
        .ofType(CommentActions.GET_USER_REPLIES)
        .map((action: CommentActions.GetUserReplies) => {
            return action.payload;
        })
        .switchMap((payload) => {
            let recordType = payload.ownerRecordType.toLowerCase();
            let url = this.baseUrl;
            switch(recordType){
                default:
                    url += recordType + 'comment/' + payload.commentId + '/replies';
                    break;
            }

            return this.httpClient.get<ShortComment[]>(url)
                .map((result) => {
                    return {
                        type: CommentActions.SET_USER_REPLIES,
                        payload: { ownerRecordType: payload.ownerRecordType, ownerRecordId: payload.commentId, userReplies: result }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })
}