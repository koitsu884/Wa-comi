import * as fromCircle from './circle.reducers';
import * as CircleActions from './circle.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Circle } from '../../_models/Circle';
import { of } from 'rxjs/observable/of';
import 'rxjs/add/operator/withLatestFrom';

@Injectable()
export class CircleEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromCircle.FeatureState>,
        private router: Router,
        private httpClient: HttpClient) { }

    @Effect()
    getCircle = this.actions$
        .ofType(CircleActions.GET_CIRCLE)
        .map((action: CircleActions.GetCircle) => {
            return action.payload;
        })
        .switchMap((id) => {
            return this.httpClient.get<Circle>(this.baseUrl + 'circle/' + id)
                .map((result) => {
                    return {
                        type: CircleActions.SET_CIRCLE,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

        @Effect()
        setCircleFilters = this.actions$
            .ofType(CircleActions.SET_CIRCLE_SEARCH_OPTIONS)
            .map(() => {
                return {
                    type: CircleActions.SEARCH_CIRCLE,
                }
            })
    
        @Effect()
        setCirclePagination = this.actions$
            .ofType(CircleActions.SET_CIRCLE_PAGE)
            .map(() => {
                return {
                    type: CircleActions.SEARCH_CIRCLE,
                }
            })

        @Effect()
        searchCircle = this.actions$
            .ofType(CircleActions.SEARCH_CIRCLE)
            .withLatestFrom(this.store$)
            .switchMap(([actions, circleState]) => {
                let Params = new HttpParams();
                if (circleState.circle.searchParam.categoryId > 0)
                    Params = Params.append('categoryId', circleState.circle.searchParam.categoryId.toString());
                if (circleState.circle.searchParam.cityId > 0)
                    Params = Params.append('cityId', circleState.circle.searchParam.cityId.toString());
                if (circleState.circle.pagination) {
                    Params = Params.append('pageNumber', circleState.circle.pagination.currentPage.toString());
                    Params = Params.append('pageSize', circleState.circle.pagination.itemsPerPage.toString());
                }
    
                return this.httpClient.post<Circle[]>(
                    this.baseUrl + 'circle/search',
                    circleState.circle.searchParam,
                    { params: Params, observe: 'response' }
                )
                    .map((response) => {
                        return {
                            type: CircleActions.SET_CIRCLE_SEARCH_RESULT,
                            payload: {
                                circles: response.body,
                                pagination: JSON.parse(response.headers.get("Pagination"))
                            }
                        }
                    })
                    .catch((error: string) => {
                        return of({ type: GlobalActions.FAILED, payload: error })
                    })
            })

            //Use global
    // @Effect()
    // tryAddCircle = this.actions$
    //     .ofType(CircleActions.TRY_ADD_CIRCLE)
    //     .map((action: CircleActions.TryAddCircle) => {
    //         return action.payload;
    //     })
    //     .switchMap((payload) => {
    //         return this.httpClient.post<Circle>(this.baseUrl + 'circle',
    //             payload.circle,
    //             {
    //                 headers: new HttpHeaders().set('Content-Type', 'application/json')
    //             })
    //             .map((result) => {
    //                 if (payload.formData == null) {
    //                     this.router.navigate(['/user/posts/' + payload.circle.id]);
    //                     return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
    //                 }

    //                 return {
    //                     type: GlobalActions.TRY_ADD_PHOTOS,
    //                     payload: {
    //                         recordType: 'circle',
    //                         recordId: result.id,
    //                         formData: payload.formData,
    //                         callbackLocation: '/user/posts/' + payload.circle.id
    //                     }
    //                 };
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

    // @Effect()
    // updateCircle = this.actions$
    //     .ofType(CircleActions.UPDATE_CIRCLE)
    //     .map((action: CircleActions.UpdateCircle) => {
    //         return action.payload
    //     })
    //     .switchMap((circle) => {
    //         return this.httpClient.put(this.baseUrl + 'circle',
    //             circle,
    //             {
    //                 headers: new HttpHeaders().set('Content-Type', 'application/json')
    //             })
    //             .map(() => {
    //                 this.router.navigate(['/users/posts', circle.appUserId]);
    //                 return {
    //                     type: GlobalActions.SUCCESS, payload: "更新しました"
    //                 };
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             })
    //     });

    // @Effect()
    // tryDeleteCircle = this.actions$
    //     .ofType(CircleActions.TRY_DELETE_CIRCLE)
    //     .switchMap((actions: CircleActions.TryDeleteCircle) => {
    //         return this.httpClient.delete(this.baseUrl + 'circle/' + actions.payload,
    //             {
    //                 headers: new HttpHeaders().set('Content-Type', 'application/json')
    //             })
    //             .map(() => {
    //                 this.router.navigate(['/circle']);
    //                 return {
    //                     type: GlobalActions.SUCCESS, payload: "削除しました"
    //                 };
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

    

}