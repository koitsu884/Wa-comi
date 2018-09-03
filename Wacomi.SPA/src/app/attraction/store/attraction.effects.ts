import * as fromAttraction from './attraction.reducers';
import * as AttractionActions from './attraction.actions';
import * as GlobalActions from '../../store/global.actions';
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Actions, Effect } from "@ngrx/effects";
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { AlertifyService } from '../../_services/alertify.service';
import { ModalService } from '../../_services/modal.service';
import { Attraction } from '../../_models/Attraction';
import { of } from 'rxjs/observable/of';

@Injectable()
export class AttractionEffects {
    baseUrl = environment.apiUrl;
    readonly CLANSEEK_MAX = 5;
    constructor(private actions$: Actions,
        private store$: Store<fromAttraction.FeatureState>,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService,
        private modal: ModalService) { }

    @Effect()
    getAttraction = this.actions$
        .ofType(AttractionActions.GET_ATTRACTION)
        .map((action: AttractionActions.GetAttraction) => {
            return action.payload;
        })
        .switchMap((id) => {
            return this.httpClient.get<Attraction>(this.baseUrl + 'attraction/' + id)
                .map((result) => {
                    return {
                        type: AttractionActions.SET_ATTRACTION,
                        payload: result
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    searchAttraction = this.actions$
        .ofType(AttractionActions.SEARCH_ATTRACTION)
        .map((action: AttractionActions.SearchAttraction) => { return action.payload })
        .switchMap((payload) => {
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

            return this.httpClient.get<Attraction[]>(this.baseUrl + 'attraction', { params: Params, observe: 'response' })
                .map((response) => {
                    // console.log(response.body);
                    return {
                        type: AttractionActions.SET_ATTRACTION_SEARCH_RESULT,
                        payload: {
                            attractions: response.body,
                            //   pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        }
        )

    @Effect()
    tryAddAttraction = this.actions$
        .ofType(AttractionActions.TRY_ADD_ATTRACTION)
        .map((action: AttractionActions.TryAddAttraction) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.post<Attraction>(this.baseUrl + 'attraction',
                payload.attraction,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map((result) => {
                    if (payload.formData == null) {
                        this.router.navigate(['/attraction']);
                        return { type: GlobalActions.SUCCESS, payload: "投稿しました" };
                    }

                    return { type: GlobalActions.TRY_ADD_PHOTOS, payload: { recordType: 'attraction', recordId: result.id, formData: payload.formData } };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    updateAttraction = this.actions$
        .ofType(AttractionActions.UPDATE_ATTRACTION)
        .map((action: AttractionActions.UpdateAttraction) => {
            return action.payload
        })
        .switchMap((attraction) => {
            return this.httpClient.put(this.baseUrl + 'attraction',
                attraction,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.router.navigate(['/attraction']);
                    return {
                        type: GlobalActions.SUCCESS, payload: "更新しました"
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        });

    @Effect()
    tryDeleteAttraction = this.actions$
        .ofType(AttractionActions.TRY_DELETE_ATTRACTION)
        .switchMap((actions: AttractionActions.TryDeleteAttraction) => {
            return this.httpClient.delete(this.baseUrl + 'attraction/' + actions.payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.router.navigate(['/attraction']);
                    return {
                        type: GlobalActions.SUCCESS, payload: "削除しました"
                    };
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })



}