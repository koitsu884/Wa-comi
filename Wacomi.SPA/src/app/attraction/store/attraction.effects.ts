
import {withLatestFrom, catchError, switchMap, map} from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import * as fromAttraction from './attraction.reducers';
import * as AttractionActions from './attraction.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Attraction } from '../../_models/Attraction';
import { AttractionReview } from '../../_models/AttractionReview';


@Injectable()
export class AttractionEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromAttraction.FeatureState>,
        private router: Router,
        private httpClient: HttpClient) { }

    @Effect()
    getAttraction = this.actions$
        .ofType(AttractionActions.GET_ATTRACTION).pipe(
        map((action: AttractionActions.GetAttraction) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<Attraction>(this.baseUrl + 'attraction/' + id).pipe(
                map((result) => {
                    return {
                        type: AttractionActions.SET_ATTRACTION,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    likeAttraction = this.actions$
        .ofType(AttractionActions.LIKE_ATTRACTION).pipe(
        map((action: AttractionActions.LikeAttraction) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post(this.baseUrl + 'attraction/like', payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }
            ).pipe(map(() => {
                return {
                    type: GlobalActions.SUCCESS, payload: "良いね！しました"
                };
            }),catchError((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            }),)
        }),)

    @Effect()
    searchAttraction = this.actions$
        .ofType(AttractionActions.SEARCH_ATTRACTION).pipe(
        map((action: AttractionActions.SearchAttraction) => { return action.payload }),
        switchMap((payload) => {
            let Params = new HttpParams();
            if (payload.categories && payload.categories.length > 0)
                Params = Params.append('categories', payload.categories.join(','));
            if (payload.cityId > 0)
                Params = Params.append('cityId', payload.cityId.toString());
            if (payload.appUserId > 0)
                Params = Params.append('appUserId', payload.appUserId.toString());
            // if (clanState.clan.pagination) {
            //     Params = Params.append('pageNumber', clanState.clan.pagination.currentPage.toString());
            //     Params = Params.append('pageSize', clanState.clan.pagination.itemsPerPage.toString());
            // }

            return this.httpClient.get<Attraction[]>(this.baseUrl + 'attraction', { params: Params, observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: AttractionActions.SET_ATTRACTION_SEARCH_RESULT,
                        payload: {
                            attractions: response.body,
                            //   pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }
        ),)

    @Effect()
    tryAddAttraction = this.actions$
        .ofType(AttractionActions.TRY_ADD_ATTRACTION).pipe(
        map((action: AttractionActions.TryAddAttraction) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<Attraction>(this.baseUrl + 'attraction',
                payload.attraction,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((result) => {
                    if (payload.formData == null) {
                        this.router.navigate(['/user/posts/' + payload.attraction.id]);
                        return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
                    }

                    return {
                        type: GlobalActions.TRY_ADD_PHOTOS,
                        payload: {
                            recordType: 'attraction',
                            recordId: result.id,
                            formData: payload.formData,
                            callbackLocation: '/user/posts/' + payload.attraction.id
                        }
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    updateAttraction = this.actions$
        .ofType(AttractionActions.UPDATE_ATTRACTION).pipe(
        map((action: AttractionActions.UpdateAttraction) => {
            return action.payload
        }),
        switchMap((attraction) => {
            return this.httpClient.put(this.baseUrl + 'attraction',
                attraction,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    this.router.navigate(['/users/posts', attraction.appUserId]);
                    return {
                        type: GlobalActions.SUCCESS, payload: "更新しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    tryDeleteAttraction = this.actions$
        .ofType(AttractionActions.TRY_DELETE_ATTRACTION).pipe(
        switchMap((actions: AttractionActions.TryDeleteAttraction) => {
            return this.httpClient.delete(this.baseUrl + 'attraction/' + actions.payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    this.router.navigate(['/attraction']);
                    return {
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }))

    @Effect()
    getAttractionReview = this.actions$
        .ofType(AttractionActions.GET_ATTRACTION_REVIEW).pipe(
        map((action: AttractionActions.GetAttractionReview) => {
            return action.payload;
        }),
        switchMap((id) => {
            return this.httpClient.get<AttractionReview>(this.baseUrl + 'attractionreview/' + id).pipe(
                map((result) => {
                    return {
                        type: AttractionActions.SET_ATTRACTION_REVIEW,
                        payload: result
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    tryAddAttractionReview = this.actions$
        .ofType(AttractionActions.TRY_ADD_ATTRACTION_REVIEW).pipe(
        map((action: AttractionActions.TryAddAttractionReview) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post<AttractionReview>(this.baseUrl + 'attractionreview',
                payload.attractionReview,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map((result) => {
                    if (payload.formData == null) {
                        this.router.navigate(['/attraction']);
                        return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
                    }

                    return {
                        type: GlobalActions.TRY_ADD_PHOTOS,
                        payload: {
                            recordType: 'attractionreview',
                            recordId: result.id,
                            formData: payload.formData,
                            callbackLocation: '/attraction/detail/' + result.attractionId
                        }
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),);
        }),)

    @Effect()
    updateAttractionReview = this.actions$
        .ofType(AttractionActions.UPDATE_ATTRACTION_REVIEW).pipe(
        map((action: AttractionActions.UpdateAttractionReview) => {
            return action.payload
        }),
        withLatestFrom(this.store$),
        switchMap(([attractionReview, attractionState]) => {
            return this.httpClient.put(this.baseUrl + 'attractionreview',
                attractionReview,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    //this.router.navigate(['/attraction/detail/', attractionState.attraction.selectedAttraction.id]);
                    this.router.navigate(['/users/posts/', attractionReview.appUserId]);
                    return {
                        type: GlobalActions.SUCCESS, payload: "更新しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }),);

    @Effect()
    tryDeleteAttractionReview = this.actions$
        .ofType(AttractionActions.TRY_DELETE_ATTRACTION_REVIEW).pipe(
        map((actions: AttractionActions.TryDeleteAttractionReview) => { return actions.payload }),
        withLatestFrom(this.store$),
        switchMap(([id, attractionState]) => {
            return this.httpClient.delete(this.baseUrl + 'attractionreview/' + id,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }).pipe(
                map(() => {
                    this.router.navigate(['/attraction/detail/', attractionState.attraction.selectedAttraction.id]);
                    return {
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    };
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error });
                }),)
        }),)
    //  .switchMap((actions: AttractionActions.TryDeleteAttractionReview) => {
    //     return this.httpClient.delete(this.baseUrl + 'attractionreview/' + actions.payload,
    //         {
    //             headers: new HttpHeaders().set('Content-Type', 'application/json')
    //         })
    //         .map(() => {
    //             this.router.navigate['/attraction/detail/' + result.attractionId]);
    //             return {
    //                 type: GlobalActions.SUCCESS, payload: "削除しました"
    //             };
    //         })
    //         .catch((error: string) => {
    //             return of({ type: GlobalActions.FAILED, payload: error })
    //         });
    // })

    @Effect()
    getAttractionReviewList = this.actions$
        .ofType(AttractionActions.GET_ATTRACTION_REVIEW_LIST).pipe(
        map((action: AttractionActions.GetAttractionReviewList) => { return action.payload }),
        switchMap((payload) => {
            // let Params = new HttpParams();
            // Params = Params.append('id', payload.toString());
            return this.httpClient.get<AttractionReview[]>(this.baseUrl + 'attraction/' + payload + '/review', { observe: 'response' }).pipe(
                map((response) => {
                    return {
                        type: AttractionActions.SET_ATTRACTION_REVIEW_LIST,
                        payload: {
                            attractionReviewList: response.body,
                            //   pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                }),
                catchError((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                }),)
        }
        ),)

    @Effect()
    likeAttractionReview = this.actions$
        .ofType(AttractionActions.LIKE_ATTRACTION_REVIEW).pipe(
        map((action: AttractionActions.LikeAttractionReview) => {
            return action.payload;
        }),
        switchMap((payload) => {
            return this.httpClient.post(this.baseUrl + 'attractionreview/like', payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }
            ).pipe(map(() => {
                return {
                    type: GlobalActions.SUCCESS, payload: "良いね！しました"
                };
            }),catchError((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            }),)
        }),)

}