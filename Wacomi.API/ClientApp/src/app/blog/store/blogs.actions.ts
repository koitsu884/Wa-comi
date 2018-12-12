import { Action } from "@ngrx/store";
import { Blog } from "../../_models/Blog";
import { BlogFeed } from "../../_models/BlogFeed";
import { Pagination } from "../../_models/Pagination";
import { ShortComment } from "../../_models/ShortComment";

export const GET_BLOG = 'GET_BLOG';
export const SET_BLOG = 'SET_BLOG';
export const TRY_ADDBLOG = 'TRY_ADDBLOG';
// export const TRY_ADD_BLOG_PHOTO = 'TRY_ADD_BLOG_PHOTO';
export const TRY_DELETEBLOG = 'TRY_DELETEBLOG';
export const ADD_BLOG = 'ADD_BLOG';
export const UPDATE_BLOG = 'UPDATE_BLOG';
export const DELETE_BLOG = 'DELETE_BLOG';
export const CLEAR_BLOG = 'CLEAR_BLOG';
export const SET_SELECTED_BLOG = 'SET_SELECTED_BLOG';

export const TRY_DELETE_FEED = 'TRY_DELETE_FEED';
export const DELETE_FEED = 'DELETE_FEED';
export const SET_FEED_SEARCH_CATEGORY = 'SET_FEED_SEARCH_CATEGORY';
export const SET_SEARCH_USER_ID = 'SET_SEARCH_USER_ID';
export const SET_FEED_SEARCH_PAGE = 'SET_FEED_SEARCH_PAGE';
export const SEARCH_FEEDS = 'SEARCH_FEEDS';
export const SEARCH_FEED_BY_ID = 'SEARCH_FEED_BY_ID';
export const SET_FEED_SEARCH_RESULT = 'SET_FEED_SEARCH_RESULT';
export const LIKE_FEED = 'LIKE_FEED';
export const SET_LIKED_FLAG = 'SET_LIKED_FLAG';

export const GET_FEED_COMMENTS = 'GET_FEED_COMMENTS';
export const SET_FEED_COMMENTS = 'SET_FEED_COMMENTS';
export const TRY_ADD_FEED_COMMENT = 'TRY_ADD_FEED_COMMENT';
export const TRY_DELETE_FEED_COMMENT = 'TRY_DELETE_FEED_COMMENT';
export const TOGGLE_COMMENT_FORM = "TOGGLE_COMMENT_FORM";

// export const GET_LIKED_FEED_NUMBER_LIST = 'GET_LIKED_FEED_NUMBER_LIST';
// export const SET_LIKED_FEED_NUMBER_LIST = 'SET_LIKED_FEED_NUMBER_LIST';



export class TryAddBlog implements Action {
    readonly type = TRY_ADDBLOG;

    constructor(public payload:{blog: Blog, photo:File}){}
}

// export class TryAddBlogPhoto implements Action {
//     readonly type = TRY_ADD_BLOG_PHOTO;

//     constructor(public payload:{blogId: number, photo:File}){}
// }

export class TryDeleteBlog implements Action {
    readonly type = TRY_DELETEBLOG;

    constructor(public payload: number) {}
}

export class GetBlog implements Action {
    readonly type = GET_BLOG;
    constructor(public payload: number) {}
}

export class SetBlog implements Action {
    readonly type = SET_BLOG;
    constructor(public payload: Blog[]) {}
}

export class SetSelectedBlog implements Action {
    readonly type = SET_SELECTED_BLOG;
    constructor(public payload: Blog) {}
}

export class AddBlog implements Action {
    readonly type = ADD_BLOG;
    constructor(public payload:Blog) {}
}

export class UpdateBlog implements Action {
    readonly type = UPDATE_BLOG;

    constructor(public payload:{blog: Blog, photo:File}) {}
}

export class DeleteBlog implements Action {
    readonly type = DELETE_BLOG;

    constructor(public payload: number) {}
}

export class ClearBlog implements Action {
    readonly type = CLEAR_BLOG;

    constructor() {}
}

export class TryDeleteFeed implements Action {
    readonly type = TRY_DELETE_FEED;

    constructor(public payload: BlogFeed){}
}

export class DeleteFeed implements Action {
    readonly type = DELETE_FEED;

    constructor(public payload: BlogFeed){}
}

export class SetFeedSearchCategory implements Action {
    readonly type = SET_FEED_SEARCH_CATEGORY;
    constructor(public payload: string){}
}

export class SetSearchUserId implements Action {
    readonly type = SET_SEARCH_USER_ID;
    constructor(public payload: number){}
}

export class SetFeedSearchPage implements Action {
    readonly type = SET_FEED_SEARCH_PAGE;
    constructor(public payload: number){}
}

export class SearchFeeds implements Action {
    readonly type = SEARCH_FEEDS;
    constructor(){}
}

export class SearchFeedById implements Action {
    readonly type = SEARCH_FEED_BY_ID;
    constructor(public payload: number){}
}

export class SetFeedSearchResult implements Action {
    readonly type = SET_FEED_SEARCH_RESULT;
    constructor(public payload: {blogFeeds: BlogFeed[], pagination: Pagination}){}
}

export class LikeFeed implements Action {
    readonly type = LIKE_FEED;
    constructor(public payload: {blogFeedId: number, supportAppUserId: number}){};
}

export class SetLikedFlag implements Action {
    readonly type = SET_LIKED_FLAG;
    constructor(public payload: number){}; //blogFeedId
}

export class GetFeedComments implements Action {
    readonly type = GET_FEED_COMMENTS;
    constructor(public payload: number){}; //blogFeedId
}

export class SetFeedComments implements Action {
    readonly type = SET_FEED_COMMENTS;
    constructor(public payload: {blogFeedId: number, comments: ShortComment[]}){};
}

export class TryAddFeedComment implements Action {
    readonly type = TRY_ADD_FEED_COMMENT;
    constructor(public payload: {blogFeedId: number, appUserId: number, comment:string}){};
}

export class TryDeleteFeedComment implements Action {
    readonly type = TRY_DELETE_FEED_COMMENT;
    constructor(public payload: {blogFeedId: number, feedCommentId: number}){};
}

export class ToggleCommentForm implements Action {
    readonly type = TOGGLE_COMMENT_FORM;
    constructor(public payload: number){};
}



export type BlogActions = TryAddBlog 
                        //    | TryAddBlogPhoto
                           | TryDeleteBlog 
                           | GetBlog 
                           | SetBlog 
                           | AddBlog 
                           | UpdateBlog 
                           | DeleteBlog 
                           | ClearBlog
                           | TryDeleteFeed
                           | DeleteFeed
                           | SearchFeeds
                           | SearchFeedById
                           | SetFeedSearchCategory
                           | SetSearchUserId
                           | SetFeedSearchPage
                           | SetFeedSearchResult
                           | LikeFeed
                           | SetLikedFlag
                           | GetFeedComments
                           | SetFeedComments
                           | TryAddFeedComment
                           | TryDeleteFeedComment
                           | ToggleCommentForm;