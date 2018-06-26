import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
// import 'rxjs/add/operator/of';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';

import * as BlogActions from './blogs.actions';
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { AlertifyService } from "../../_services/alertify.service";
import { Observable } from "rxjs/Observable";

import { Action } from "@ngrx/store";
import { of } from "rxjs/observable/of";
import { Blog } from "../../_models/Blog";
@Injectable()
export class BlogEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private router: Router,
        private httpClient: HttpClient,
        private alertify: AlertifyService) { }

    @Effect()
    getBlog = this.actions$
        .ofType(BlogActions.GET_BLOG)
        .map((action: BlogActions.GetBlog) => {
            return action.payload;
        })
        .switchMap((appUserId) => {
            return this.httpClient.get<Blog[]>(this.baseUrl + 'blog/user/' + appUserId)
            .map((result) => {
                return {
                    type: BlogActions.SET_BLOG,
                    payload: result
                }
            })
            .catch((error: string) => {
                return of({ type: 'FAILED', payload: error })
            });
        })

    @Effect()
    tryAddBlog = this.actions$
        .ofType(BlogActions.TRY_ADDBLOG)
        .map((action: BlogActions.TryAddBlog) => {
            return action.payload
        })
        .switchMap((newBlog) => {
            return this.httpClient.post<Blog>(this.baseUrl + 'blog',
                newBlog,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map((result) => {
                    this.alertify.success("ブログを追加しました");
                    return {
                        type: BlogActions.ADD_BLOG, payload: result
                    };
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                });
        })

    @Effect()
    updateBlog = this.actions$
        .ofType(BlogActions.UPDATE_BLOG)
        .map((action: BlogActions.UpdateBlog) => {
            return action.payload
        })
        .switchMap((blog) => {
            return this.httpClient.put(this.baseUrl + 'blog/',
                blog,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    this.alertify.success("更新しました");
                    return {
                        type: BlogActions.GET_BLOG, payload: blog.ownerId
                    };
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                })
        });

    @Effect()
    tryDeleteBlog = this.actions$
        .ofType(BlogActions.TRY_DELETEBLOG)
        .map((action: BlogActions.TryDeleteBlog) => {
            return action.payload
        })
        .switchMap((id) => {
            return this.httpClient.delete(this.baseUrl + 'blog/' + id)
                .map(() => {
                    this.alertify.success("削除しました");
                    return {
                        type: BlogActions.DELETE_BLOG, payload: id
                    };
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                })
        });

}