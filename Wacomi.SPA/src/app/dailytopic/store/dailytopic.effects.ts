
import {catchError, mergeMap, switchMap, map} from 'rxjs/operators';
import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Actions, Effect } from "@ngrx/effects";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";
import * as TopicActions from "./dailytopic.actions";
import * as GlobalActions from "../../store/global.actions";
import { DailyTopic } from "../../_models/DailyTopic";
import { of } from "rxjs";
import { TopicComment } from "../../_models/TopicComment";
import { TopicCommentFeel } from "../../_models/TopicCommentFeel";
import { TopicReply } from "../../_models/TopicReply";
import { ShortComment } from "../../_models/ShortComment";

@Injectable()
export class DailyTopicEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    @Effect()
    getTopicList = this.actions$
        .ofType(TopicActions.GET_TOPIC_LIST).pipe(
        map((action: TopicActions.GetTopicList) => {
            return action.payload;
        }),
        switchMap((userId) => {
            return this.httpClient.get<DailyTopic[]>(this.baseUrl + 'dailytopic').pipe(
                mergeMap((result) => {
                    let returnValues: Array<any> = [{ 
                        type: TopicActions.SET_TOPIC_LIST,
                        payload: result 
                    }];

                    if(userId){
                        returnValues.push({
                            type: TopicActions.GET_LIKED_TOPIC_LIST,
                            payload: userId
                        });
                    }
                    return returnValues;
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

        @Effect()
        getLikedTopicList = this.actions$
            .ofType(TopicActions.GET_LIKED_TOPIC_LIST).pipe(
            map((action: TopicActions.GetLikedTopicList) => {
                return action.payload;
            }),
            switchMap((userId) => {
                return this.httpClient.get<number[]>(this.baseUrl + 'topiclike/' + userId).pipe(
                    map((result) => {
                        return {
                            type: TopicActions.SET_LIKED_TOPIC_LIST,
                            payload: result
                        }
                    }),
                    catchError((error: string) => {
                        return of({ type: GlobalActions.FAILED, payload: error })
                    }),);
            }),)

    @Effect()
    tryAddTopic = this.actions$
        .ofType(TopicActions.TRY_ADD_TOPIC).pipe(
        map((action: TopicActions.TryAddTopic) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<DailyTopic>(this.baseUrl + 'dailytopic',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((newDailyTopic) => {
                    newDailyTopic.likedCount = 0;
                    newDailyTopic.isLiked = false;
                    return {
                        type: TopicActions.ADD_TOPIC, payload: newDailyTopic
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryDeleteTopic = this.actions$
        .ofType(TopicActions.TRY_DELETE_TOPIC).pipe(
        map((action: TopicActions.TryDeleteTopic) => {
            return action.payload
        }),
        switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + 'dailytopic?userId=' + payload.userId + '&recordId=' + payload.id).pipe(
                map(() => {
                    return {
                        type: TopicActions.DELETE_TOPIC, payload: payload.id
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    likeTopic = this.actions$
        .ofType(TopicActions.LIKE_TOPIC).pipe(
        map((action: TopicActions.LikeTopic) => {
            return action.payload
        }),
        switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + 'topiclike',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    return {
                        type: GlobalActions.SUCCESS, payload: null
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    //==================================================
    @Effect()
    getTopicComments = this.actions$
        .ofType(TopicActions.GET_TOPIC_COMMENTS).pipe(
        map((action: TopicActions.GetTopicComments) => {
            return action.payload;
        }),
        switchMap((appUserId) => {
            return this.httpClient.get<TopicComment[]>(this.baseUrl + 'dailytopiccomment/list').pipe(
                mergeMap((result) => {
                    return [{
                        type: TopicActions.SET_TOPIC_COMMENTS,
                        payload: { comments: result, appUserId: appUserId }
                    },
                    {
                        type: TopicActions.GET_COMMENT_FEELINGS,
                        payload: appUserId
                    }
                    ]
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)
    @Effect()
    tryAddComment = this.actions$
        .ofType(TopicActions.TRY_ADD_TOPIC_COMMENT).pipe(
        map((action: TopicActions.TryAddTopicComment) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<TopicComment>(this.baseUrl + 'dailytopiccomment',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap((newComment) => {
                    return [
                        {
                            type: TopicActions.ADD_TOPIC_COMMENT, payload: newComment
                        },
                        {
                            type: TopicActions.GET_TOPIC_COMMENTS, payload: payload.appUserId
                        },
                        {
                            type: GlobalActions.SUCCESS, payload: "トピックコメントを投稿しました"
                        }
                    ]
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryDeleteTopicComment = this.actions$
        .ofType(TopicActions.TRY_DELETE_TOPIC_COMMENT).pipe(
        map((action: TopicActions.TryDeleteTopicComment) => {
            return action.payload //topicId
        }),
        switchMap((topicId) => {
            return this.httpClient.delete(this.baseUrl + 'dailytopiccomment/' + topicId).pipe(
                mergeMap(() => {
                    return [
                        {
                            type: TopicActions.DELETE_TOPIC_COMMENT, payload: topicId
                        },
                        { type: GlobalActions.SUCCESS, payload: "削除しました" }
                    ];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    getCommentFeelings = this.actions$
        .ofType(TopicActions.GET_COMMENT_FEELINGS).pipe(
        map((action: TopicActions.GetCommentFeelings) => {
            return action.payload;
        }),
        switchMap((appUserId) => {
            if (!appUserId)
            {
                return of({type: TopicActions.SET_COMMENT_FEELINGS,payload: null});
            }

            return this.httpClient.get<TopicCommentFeel[]>(this.baseUrl + 'topiccommentfeel/' + appUserId).pipe(
                map((result) => {
                    return {
                        type: TopicActions.SET_COMMENT_FEELINGS,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }));
        })
        )

    @Effect()
    tryAddCommentFeeling = this.actions$
        .ofType(TopicActions.TRY_ADD_COMMENT_FEELING).pipe(
        map((action: TopicActions.TryAddCommentFeeling) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<TopicCommentFeel>(this.baseUrl + 'topiccommentfeel',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((newFeeling) => {
                    return {
                        type: TopicActions.ADD_COMMENT_FEELING, payload: newFeeling
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    getTopicReplies = this.actions$
        .ofType(TopicActions.GET_TOPIC_REPLIES).pipe(
        map((action: TopicActions.GetTopicReplies) => {
            return action.payload.commentId;
        }),
        switchMap((commentId) => {
            return this.httpClient.get<ShortComment[]>(this.baseUrl + 'dailytopicreply/topic/' + commentId).pipe(
                map((result) => {
                    return {
                        type: TopicActions.SET_TOPIC_REPLIES,
                        payload: { commentId: commentId, topicReplies: result }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryAddTopicReply = this.actions$
        .ofType(TopicActions.TRY_ADD_TOPIC_REPLY).pipe(
        map((action: TopicActions.TryAddTopicReply) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<TopicReply>(this.baseUrl + 'dailytopicreply',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
                    return [{
                        type: TopicActions.GET_TOPIC_REPLIES, payload: {commentId:payload.topicCommentId}
                    },
                    {
                        type: GlobalActions.SUCCESS, payload: "コメントを送信しました"
                    }
                    ];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryDeleteTopicReply = this.actions$
        .ofType(TopicActions.TRY_DELETE_TOPIC_REPLY).pipe(
        map((action: TopicActions.TryDeleteTopicReply) => {
            return action.payload 
        }),
        switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + 'dailytopicreply/' + payload.topicReplyId).pipe(
                mergeMap(() => {
                    return [
                        {
                            type: TopicActions.GET_TOPIC_REPLIES, payload:{commentId:payload.topicCommentId}
                        },
                        { type: GlobalActions.SUCCESS, payload: "削除しました" }
                    ];
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);
}