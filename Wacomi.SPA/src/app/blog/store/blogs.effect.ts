import { Injectable } from "@angular/core";
import { Actions, Effect } from '@ngrx/effects';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
// import 'rxjs/add/operator/of';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/withLatestFrom';

import * as BlogActions from './blogs.actions';
import * as GlobalActions from "../../store/global.actions";
import * as fromBlog from './blogs.reducers';
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { AlertifyService } from "../../_services/alertify.service";

import { Store } from "@ngrx/store";
import { of } from "rxjs/observable/of";
import { Blog } from "../../_models/Blog";
import { BlogFeed } from "../../_models/BlogFeed";
import { ShortComment } from "../../_models/ShortComment";
import { ModalService } from "../../_services/modal.service";
import { Location } from "@angular/common";

@Injectable()
export class BlogEffects {
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions,
        private store$: Store<fromBlog.FeatureState>,
        private httpClient: HttpClient,
        private location: Location,
        private alertify: AlertifyService) { }

    @Effect()
    getBlog = this.actions$
        .ofType(BlogActions.GET_BLOG)
        .map((action: BlogActions.GetBlog) => {
            return action.payload;
        })
        .switchMap((appUserId) => {
            return this.httpClient.get<Blog[]>(this.baseUrl + 'blog/user/' + appUserId + '?includeFeeds=true')
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
        .switchMap((payload) => {
            return this.httpClient.post<Blog>(this.baseUrl + 'blog',
                payload.blog,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .mergeMap((result) => {
                    let returnValues: Array<any> = [{ type: BlogActions.ADD_BLOG, payload: result }];
                    if (payload.photo != null) {
                        var formData = new FormData();
                        formData.append("files", payload.photo);

                        //returnValues.push({ type: BlogActions.TRY_ADD_BLOG_PHOTO, payload: { blogId: result.id, photo: payload.photo } });
                        returnValues.push({
                            type: GlobalActions.TRY_ADD_PHOTOS,
                            payload: {
                                recordType: 'blog',
                                recordId: result.id,
                                formData: formData,
                                callbackLocation: '/blog'
                            }});
                    }
                    else {
                        this.location.back();
                    }

                    this.alertify.success("ブログを追加しました");
                    return returnValues;
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
        .switchMap((payload) => {
            return this.httpClient.put(this.baseUrl + 'blog/',
                payload.blog,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                })
                .map(() => {
                    //this.alertify.success("更新しました");
                    if (payload.photo) {
                        this.alertify.success("更新しました");
                        var formData = new FormData();
                        formData.append("files", payload.photo);

                        //returnValues.push({ type: BlogActions.TRY_ADD_BLOG_PHOTO, payload: { blogId: result.id, photo: payload.photo } });
                        return {
                            type: GlobalActions.TRY_ADD_PHOTOS,
                            payload: {
                                recordType: 'blog',
                                recordId: payload.blog.id,
                                formData: formData,
                                callbackLocation: '/blog'
                            }
                        };

                       // return { type: BlogActions.TRY_ADD_BLOG_PHOTO, payload: { blogId: payload.blog.id, photo: payload.photo } }
                    }

                    this.location.back();
                    return { type: GlobalActions.SUCCESS, payload: "更新しました" };
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

    @Effect()
    tryDeleteFeed = this.actions$
        .ofType(BlogActions.TRY_DELETE_FEED)
        .map((action: BlogActions.TryDeleteFeed) => {
            return action.payload
        })
        .switchMap((blogFeed) => {
            return this.httpClient.put(this.baseUrl + 'blogfeed/' + blogFeed.id + '/disable', null)
                .map(() => {
                    this.alertify.success("削除しました");
                    return {
                        type: BlogActions.DELETE_FEED, payload: blogFeed
                    };
                })
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                })
        });

    @Effect()
    blogSearchFilter = this.actions$
        .ofType(BlogActions.SET_FEED_SEARCH_CATEGORY)
        .map(() => {
            return {
                type: BlogActions.SEARCH_FEEDS,
            }
        })

    @Effect()
    setOnlyMineFlag = this.actions$
        .ofType(BlogActions.SET_SEARCH_USER_ID)
        .map(() => {
            return {
                type: BlogActions.SEARCH_FEEDS,
            }
        })

    @Effect()
    searchFeedById = this.actions$
        .ofType(BlogActions.SEARCH_FEED_BY_ID)
        .map((action: BlogActions.SearchFeedById) => {return action.payload;})
        .switchMap((feedId) => {
            return this.httpClient.get<BlogFeed>(this.baseUrl + 'blogfeed/' + feedId)
                .map((blogFeed) => {
                    var blogFeeds = [blogFeed];
                    return {
                        type: BlogActions.SET_FEED_SEARCH_RESULT,
                        payload: {
                            blogFeeds: blogFeeds,
                            pagination: null
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        })

    @Effect()
    setClanSeekPagination = this.actions$
        .ofType(BlogActions.SET_FEED_SEARCH_PAGE)
        .map(() => {
            return {
                type: BlogActions.SEARCH_FEEDS,
            }
        })

    @Effect()
    searchFeeds = this.actions$
        .ofType(BlogActions.SEARCH_FEEDS)
        .withLatestFrom(this.store$)
        .switchMap(([actions, blogState]) => {
            let Params = new HttpParams();
            if (blogState.blogs.searchUserId)
                Params = Params.append('userId', blogState.blogs.searchUserId.toString());
            if (blogState.blogs.searchCategory)
                Params = Params.append('category', blogState.blogs.searchCategory);
            if (blogState.blogs.pagination) {
                Params = Params.append('pageNumber', blogState.blogs.pagination.currentPage.toString());
                Params = Params.append('pageSize', blogState.blogs.pagination.itemsPerPage.toString());
            }

            return this.httpClient.get<BlogFeed[]>(this.baseUrl + 'blogfeed', { params: Params, observe: 'response' })
                .map((response) => {
                    return {
                        type: BlogActions.SET_FEED_SEARCH_RESULT,
                        payload: {
                            blogFeeds: response.body,
                            pagination: JSON.parse(response.headers.get("Pagination"))
                        }
                    }
                })
                .catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                })
        })

    @Effect()
    likeFeed = this.actions$
        .ofType(BlogActions.LIKE_FEED)
        .map((action: BlogActions.LikeFeed) => {
            return action.payload;
        })
        .switchMap((payload) => {
            return this.httpClient.post(this.baseUrl + 'blogfeed/like', payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }
            ).map(() => {
                this.alertify.success("良いね！しました");
                return {
                    type: BlogActions.SET_LIKED_FLAG, payload: payload.blogFeedId
                };
            }).catch((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            })
        })

    @Effect()
    getFeedComments = this.actions$
        .ofType(BlogActions.GET_FEED_COMMENTS)
        .map((action: BlogActions.GetFeedComments) => {
            return action.payload;
        })
        .switchMap((blogFeedId) => {
            return this.httpClient.get<ShortComment[]>(this.baseUrl + 'blogfeed/comment?blogFeedId=' + blogFeedId)
                .map((result) => {
                    return {
                        type: BlogActions.SET_FEED_COMMENTS,
                        payload: { blogFeedId: blogFeedId, comments: result }
                    };
                }).catch((error: string) => {
                    return of({ type: GlobalActions.FAILED, payload: error })
                });
        })

    @Effect()
    tryAddFeedComment = this.actions$
        .ofType(BlogActions.TRY_ADD_FEED_COMMENT)
        .map((action: BlogActions.TryAddFeedComment) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.post(
                this.baseUrl + 'blogfeed/comment',
                payload,
                {
                    headers: new HttpHeaders().set('Content-Type', 'application/json')
                }
            ).mergeMap(() => {
                return [
                    {
                        type: BlogActions.GET_FEED_COMMENTS, payload: payload.blogFeedId
                    },
                    {
                        type: GlobalActions.SUCCESS, payload: "コメントを追加しました"
                    }
                ];
            }).catch((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            })
        })

    @Effect()
    tryDeleteFeedComment = this.actions$
        .ofType(BlogActions.TRY_DELETE_FEED_COMMENT)
        .map((action: BlogActions.TryDeleteFeedComment) => {
            return action.payload
        })
        .switchMap((payload) => {
            return this.httpClient.delete(
                this.baseUrl + 'blogfeed/comment/' + payload.feedCommentId
            ).mergeMap(() => {
                return [
                    {
                        type: GlobalActions.SUCCESS, payload: "コメントを削除しました"
                    },
                    {
                        type: BlogActions.GET_FEED_COMMENTS, payload: payload.blogFeedId
                    }
                ]
            }).catch((error: string) => {
                return of({ type: GlobalActions.FAILED, payload: error })
            })
        })

    // @Effect()
    // getLikedFeedNumberList = this.actions$
    //     .ofType(BlogActions.GET_LIKED_FEED_NUMBER_LIST)
    //     .map((action: BlogActions.GetLikedFeedNumberList) => {
    //         return action.payload;
    //     })
    //     .switchMap((userId) => {
    //         return this.httpClient.get<number[]>(this.baseUrl + 'blogfeed/like/user/' + userId)
    //             .map((result) => {
    //                 return {
    //                     type: BlogActions.SET_LIKED_FEED_NUMBER_LIST,
    //                     payload: result
    //                 }
    //             })
    //             .catch((error: string) => {
    //                 return of({ type: GlobalActions.FAILED, payload: error })
    //             });
    //     })

}