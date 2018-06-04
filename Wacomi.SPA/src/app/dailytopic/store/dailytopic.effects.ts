import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Actions, Effect } from "@ngrx/effects";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";
import * as TopicActions from "./dailytopic.actions";
import * as GlobalActions from "../../store/global.actions";
import { DailyTopic } from "../../_models/DailyTopic";
import { of } from "rxjs/observable/of";
import { TopicComment } from "../../_models/TopicComment";
import { TopicCommentFeel } from "../../_models/TopicCommentFeel";

@Injectable()
export class DailyTopicEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    @Effect()
    getTopicList = this.actions$
        .ofType(TopicActions.GET_TOPIC_LIST)
        .map((action: TopicActions.GetTopicList) => {
            return action.payload;
        })
        .switchMap((userId) => {
            return this.httpClient.get<DailyTopic[]>(this.baseUrl + 'dailytopic?userId=' + userId)
                .map((result) => {
                    return {
                        type: TopicActions.SET_TOPIC_LIST,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddTopic = this.actions$
        .ofType(TopicActions.TRY_ADD_TOPIC)
        .map((action: TopicActions.TryAddTopic) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.post<DailyTopic>(this.baseUrl + 'dailytopic',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map((newDailyTopic) => {
                    newDailyTopic.likedCount = 0;
                    newDailyTopic.isLiked = false;
                    return {
                        type: TopicActions.ADD_TOPIC, payload: newDailyTopic
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryDeleteTopic = this.actions$
        .ofType(TopicActions.TRY_DELETE_TOPIC)
        .map((action: TopicActions.TryDeleteTopic) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.delete(this.baseUrl + 'dailytopic?userId=' + payload.userId + '&recordId=' + payload.id)
                .map(() => {
                    return {
                        type: TopicActions.DELETE_TOPIC, payload: payload.id
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        });

    @Effect()
    likeTopic = this.actions$
        .ofType(TopicActions.LIKE_TOPIC)
        .map((action: TopicActions.LikeTopic) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.post<any>(this.baseUrl + 'dailytopic/like',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    return {
                        type: GlobalActions.SUCCESS, payload: null
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

        //==================================================
        @Effect()
        getTopicComments = this.actions$
            .ofType(TopicActions.GET_TOPIC_COMMENTS)
            .map((action: TopicActions.GetTopicComments) => {
                return action.payload;
            })
            .switchMap((memberId) => {
                return this.httpClient.get<TopicComment[]>(this.baseUrl + 'dailytopiccomment/list')
                    .mergeMap((result) => {
                        return [{
                            type: TopicActions.SET_TOPIC_COMMENTS,
                            payload: {comments: result, memberId: memberId}
                        },
                        {
                            type: TopicActions.GET_COMMENT_FEELINGS,
                            payload: memberId
                        }
                        ]
                    })
                    .catch((error: string) => {
                        return of({ type: GlobalActions.FAILED, payload: error })
                    });
            })
            @Effect()
            tryAddComment = this.actions$
                .ofType(TopicActions.TRY_ADD_TOPIC_COMMENT)
                .map((action: TopicActions.TryAddTopicComment) => {
                    return action.payload;
                })
                .switchMap((payload) => {
                    console.log(payload);
                    return this.httpClient.post<TopicComment>(this.baseUrl + 'dailytopiccomment',
                        payload,
                        {
                            headers: new HttpHeaders().set('Content-Type', 'application/json')
                        })
                        .mergeMap((newComment) => {
                            return [
                                {
                                    type: TopicActions.ADD_TOPIC_COMMENT, payload: newComment
                                },
                                {
                                    type: GlobalActions.SUCCESS, payload: "トピックコメントを投稿しました"
                                }
                            ]
                        })
                        .catch((error: string) => {
                            return of({ type: GlobalActions.FAILED, payload: error })
                        });
                })

                @Effect()
                tryDeleteTopicComment = this.actions$
                    .ofType(TopicActions.TRY_DELETE_TOPIC_COMMENT)
                    .map((action: TopicActions.TryDeleteTopicComment) => {
                        return action.payload //topicId
                    })
                    .switchMap((topicId) => {
                        return this.httpClient.delete(this.baseUrl + 'dailytopiccomment/' + topicId)
                            .mergeMap(() => {
                                return [
                                {
                                    type: TopicActions.DELETE_TOPIC_COMMENT, payload: topicId
                                },
                                { type: GlobalActions.SUCCESS, payload: "削除しました"}
                                ];
                            })
                            .catch((error: string) => {
                                return of({ type: GlobalActions.FAILED, payload: error })
                            })
                    });

            @Effect()
            getCommentFeelings = this.actions$
                .ofType(TopicActions.GET_COMMENT_FEELINGS)
                .map((action: TopicActions.GetCommentFeelings) => {
                    return action.payload;
                })
                .switchMap((memberId) => {
                    if(!memberId)
                    return of({
                        type: TopicActions.SET_COMMENT_FEELINGS,
                        payload: []
                    });

                    return this.httpClient.get<TopicCommentFeel[]>(this.baseUrl + 'topiccommentfeel/' + memberId)
                        .map((result) => {
                            return {
                                type: TopicActions.SET_COMMENT_FEELINGS,
                                payload: result
                            }
                        })
                        .catch((error: string) => {
                            return of({ type: GlobalActions.FAILED, payload: error })
                        });
                })

                @Effect()
                tryAddCommentFeeling = this.actions$
                    .ofType(TopicActions.TRY_ADD_COMMENT_FEELING)
                    .map((action: TopicActions.TryAddCommentFeeling) => {
                        return action.payload;
                    })
                    .switchMap((payload) => {
                        return this.httpClient.post<TopicCommentFeel>(this.baseUrl + 'topiccommentfeel',
                            payload,
                            {
                                headers: new HttpHeaders().set('Content-Type', 'application/json')
                            })
                            .map((newFeeling) => {
                                return {
                                    type: TopicActions.ADD_COMMENT_FEELING, payload: newFeeling
                                };
                            })
                            .catch((error: string) => {
                                return of({ type: GlobalActions.FAILED, payload: error })
                            });
                    })
}