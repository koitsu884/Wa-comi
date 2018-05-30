import { environment } from "../../../environments/environment";
import { Injectable } from "@angular/core";
import { Actions, Effect } from "@ngrx/effects";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { AlertifyService } from "../../_services/alertify.service";
import * as TopicActions from "./dailytopic.actions";
import * as GlobalActions from "../../store/global.actions";
import { DailyTopic } from "../../_models/DailyTopic";
import { of } from "rxjs/observable/of";

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
}