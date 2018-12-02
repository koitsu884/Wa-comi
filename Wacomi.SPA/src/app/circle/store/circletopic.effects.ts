
import {withLatestFrom, catchError, mergeMap, switchMap, map} from 'rxjs/operators';
import * as fromCircle from '../store/circle.reducers';
import * as CircleTopicActions from '../store/circletopic.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from "@ngrx/store";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { CircleTopic } from '../../_models/CircleTopic';
import { of } from 'rxjs';
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
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC).pipe(
        map((action: CircleTopicActions.GetCircleTopic) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<CircleTopic>(this.baseUrl + 'circletopic/' + id).pipe(
                mergeMap((result) => {
                    return [
                        {
                            type: CircleTopicActions.SET_CIRCLE_TOPIC,
                            payload: result
                        }
                    ]
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    updateCircleTopic = this.actions$
        .ofType(CircleTopicActions.UPDATE_CIRCLE_TOPIC).pipe(
        map((action: CircleTopicActions.UpdateCircleTopic) => {
            return action.payload
        }),
        switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + 'circletopic/',
                payload.circleTopic,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                mergeMap(() => {
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
                }),
                catchError((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                }),)
        }),);



    @Effect()
    getLatestCircleTopicMemberList = this.actions$
        .ofType(CircleTopicActions.GET_CIRCLE_TOPIC_LIST).pipe(
        map((action: CircleTopicActions.GetCircleTopicList) => {
            return action.payload;
        }),
        withLatestFrom(this.store$),
        switchMap(([payload, circleTopicState]) => {
            let Params = new HttpParams();
            if (!payload.initPage && circleTopicState.circleModule.circleTopic.pagination) {
                Params = Params.append('pageNumber', circleTopicState.circleModule.circleTopic.pagination.currentPage.toString());
                Params = Params.append('pageSize', circleTopicState.circleModule.circleTopic.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<CircleTopic[]>(this.baseUrl + 'circle/' + payload.circleId + '/topics', { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: CircleTopicActions.SET_CIRCLE_TOPIC_LIST,
                        payload: {
                            topicList: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    setCircleTopicPagination = this.actions$
        .ofType(CircleTopicActions.SET_CIRCLE_TOPIC_PAGE).pipe(
        withLatestFrom(this.store$),
        map(([action, circleTopicState]) => {
            return {
                type: CircleTopicActions.GET_CIRCLE_TOPIC_LIST,
                payload: { circleId: circleTopicState.circleModule.circle.selectedCircle.id }
            }
        }),)
}