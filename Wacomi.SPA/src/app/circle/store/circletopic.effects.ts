import * as fromCircle from '../store/circle.reducers';
import * as CircleTopicActions from '../store/circletopic.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CircleTopic } from '../../_models/CircleTopic';
import { of } from 'rxjs/observable/of';
import { Router } from '@angular/router';
import { CircleTopicComment } from '../../_models/CircleTopicComment';
import { ShortComment } from '../../_models/ShortComment';

@Injectable()
export class CircleTopicEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private store$: Store<fromCircle.FeatureState>,
        private httpClient: HttpClient) { }

    @Effect()
    getCircleTopic = this.actions$
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC)
        .map((action: CircleTopicActions.GetCircleTopic) => {
            return action.payload;
        })
        .switchMap((id) => {
            return this.httpClient.get<CircleTopic>(this.baseUrl + 'circletopic/' + id)
                .mergeMap((result) => {
                    return [
                        {
                            type: CircleTopicActions.SET_CIRCLE_TOPIC,
                            payload: result
                        }
                    ]
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    getCircleTopicCommentList = this.actions$
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC_COMMENT_LIST)
        .map((action: CircleTopicActions.GetCircleTopicCommentList) => {
            return action.payload;
        })
        .withLatestFrom(this.store$)
        .switchMap(([payload, circleMemberState]) => {
            let Params = new HttpParams();
            if (!payload.initPage && circleMemberState.circleModule.circleTopic.commentPagination) {
                Params = Params.append('pageNumber', circleMemberState.circleModule.circleTopic.commentPagination.currentPage.toString());
                Params = Params.append('pageSize', circleMemberState.circleModule.circleTopic.commentPagination.itemsPerPage.toString());
            }

            return this.httpClient.get<CircleTopicComment[]>(this.baseUrl + 'circletopic/' + payload.topicId + '/comments', { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: CircleTopicActions.SET_CIRCLE_TOPIC_COMMENT_LIST,
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
        .ofType(CircleTopicActions.GET_FORCUSED_TOPIC_COMMENT)
        .map((action: CircleTopicActions.GetForcusedTopicComment) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.get<CircleTopicComment>(this.baseUrl + 'circletopiccomment/' + payload)
                .map((circleTopicComment) => {
                    return {
                        type: CircleTopicActions.SET_CIRCLE_TOPIC_COMMENT_LIST,
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
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC_REPLIES)
        .map((action: CircleTopicActions.GetCircleTopicReplies) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.get<ShortComment[]>(this.baseUrl + 'circletopiccomment/' + payload + '/replies')
                .map((result) => {
                    return {
                        type: CircleTopicActions.SET_CIRCLE_TOPIC_REPLIES,
                        payload: { commentId: payload, topicReplies: result }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    updateCircleTopic = this.actions$
        .ofType(CircleTopicActions.UPDATE_CIRCLE_TOPIC)
        .map((action: CircleTopicActions.UpdateCircleTopic) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + 'circletopic/',
                payload.circleTopic,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap(() => {
                    //this.alertify.success("更新しました");
                    if (payload.photo) {
                        // this.alertify.success("更新しました");
                        var formData = new FormData();
                        formData.append("files", payload.photo);

                        //returnValues.push({ type: BlogActions.TRY_ADD_BLOG_PHOTO, payload: { blogId: result.id, photo: payload.photo } });
                        return [
                            { type: GlobalActions.SUCCESS, payload: "更新しました" },
                            {
                                type: GlobalActions.TRY_ADD_PHOTOS,
                                payload: {
                                    recordType: 'circletopic',
                                    recordId: payload.circleTopic.id,
                                    formData: formData,
                                    callbackLocation: '/circle/detail/' + payload.circleTopic.circleId
                                }
                            }
                        ];

                        // return { type: BlogActions.TRY_ADD_BLOG_PHOTO, payload: { blogId: payload.blog.id, photo: payload.photo } }
                    }

                    this.router.navigate(['/circle/detail', payload.circleTopic.circleId]);
                    return [{ type: GlobalActions.SUCCESS, payload: "更新しました" }];
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                })
        });



    @Effect()
    getLatestCircleTopicMemberList = this.actions$
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC_LIST)
        .map((action: CircleTopicActions.GetCircleTopicList) => {
            return action.payload;
        })
        .withLatestFrom(this.store$)
        .switchMap(([payload, circleTopicState]) => {
            let Params = new HttpParams();
            if (!payload.initPage && circleTopicState.circleModule.circleTopic.pagination) {
                Params = Params.append('pageNumber', circleTopicState.circleModule.circleTopic.pagination.currentPage.toString());
                Params = Params.append('pageSize', circleTopicState.circleModule.circleTopic.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<CircleTopic[]>(this.baseUrl + 'circle/' + payload.circleId + '/topics', { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: CircleTopicActions.SET_CIRCLE_TOPIC_LIST,
                        payload: {
                            topicList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    setCircleTopicPagination = this.actions$
        .ofType(CircleTopicActions.SET_CIRCLE_TOPIC_PAGE)
        .withLatestFrom(this.store$)
        .map(([action, circleTopicState]) => {
            return {
                type: CircleTopicActions.GET_CIRCLE_TOPIC_LIST,
                payload: { circleId: circleTopicState.circleModule.circle.selectedCircle.id }
            }
        })

    @Effect()
    setCircleTopicCommentPagination = this.actions$
        .ofType(CircleTopicActions.SET_CIRCLE_TOPIC_COMMENT_PAGE)
        .withLatestFrom(this.store$)
        .map(([action, circleTopicState]) => {
            return {
                type: CircleTopicActions.GET_CIRCLE_TOPIC_COMMENT_LIST,
                payload: { topicId: circleTopicState.circleModule.circleTopic.selectedCircleTopic.id }
            }
        })



}