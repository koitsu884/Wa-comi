import * as BlogActions from './blogs.actions';
import * as fromApp from '../../store/app.reducer';
import { Blog } from '../../_models/Blog';
import { BlogFeed } from '../../_models/BlogFeed';
import { Pagination } from '../../_models/Pagination';

export interface FeatureState extends fromApp.AppState {
    blogs: State
}

export interface State {
    blogs: Blog[];
    selectedBlog: Blog;
    //feed search
    searchCategory: string;
    pagination: Pagination,
    blogFeeds: BlogFeed[];
    likedBlogFeedList: number[];
    loading: boolean;
}

const initialState: State = {
    blogs: [],
    selectedBlog: null,
    blogFeeds: null,
    likedBlogFeedList: null,
    searchCategory: null,
    pagination: null,
    loading: false
};


export function blogReducer(state = initialState, action: BlogActions.AccountActions) {
    let tempBlogs: Blog[];
    let tempPagination: Pagination;
    switch (action.type) {
        case BlogActions.SET_BLOG:
            var tempBlog = action.payload == null ? [] : action.payload;
            // localStorage.setItem('blogs', JSON.stringify(tempBlog));
            return {
                ...state,
                blogs: tempBlog,
            }
        case BlogActions.ADD_BLOG:
            // localStorage.setItem('blogs', JSON.stringify([...state.blogs, action.payload])); 
            return {
                ...state,
                blogs: [...state.blogs, action.payload]
            };
        case BlogActions.DELETE_BLOG:
            tempBlogs = [...state.blogs];
            var index = tempBlogs.findIndex(x => x.id == action.payload);
            tempBlogs.splice(index, 1);
            // localStorage.setItem('blogs', JSON.stringify(tempBlogs)); 
            return {
                ...state,
                blogs: tempBlogs
            };
        case BlogActions.DELETE_FEED:
            const oldBlogs = [...state.blogs];
            var index = oldBlogs.findIndex(x => x.id == action.payload.blogId);
            var feedIndex = oldBlogs[index].blogFeeds.findIndex(bf => bf.id == action.payload.id);
            oldBlogs[index].blogFeeds.splice(feedIndex, 1);
            return {
                ...state,
                blogs: oldBlogs
            };
        case BlogActions.SET_FEED_SEARCH_CATEGORY:
            return {
                ...state,
                searchCategory: action.payload,
                pagination: null
            };
        case BlogActions.SET_FEED_SEARCH_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        case BlogActions.SEARCH_FEEDS:
            return {
                ...state,
                loading: true,
                blogFeeds: null
            }
        case BlogActions.SET_FEED_SEARCH_RESULT:
            return {
                ...state,
                blogFeeds: action.payload.blogFeeds,
                pagination: action.payload.pagination,
                loading: false,
            }
        case BlogActions.SET_LIKED_FLAG:
            var tempBlogFeeds = [...state.blogFeeds];
            var index = tempBlogFeeds.findIndex(x => x.id == action.payload);
            tempBlogFeeds[index].isLiked = true;
            tempBlogFeeds[index].likedCount ++;
            return {
                ...state,
                blogFeeds: tempBlogFeeds
            }
        case BlogActions.SET_FEED_COMMENTS:
            var tempBlogFeeds = [...state.blogFeeds];
            var index = tempBlogFeeds.findIndex(x => x.id == action.payload.blogFeedId);
            tempBlogFeeds[index].shortComments = action.payload.comments;
            tempBlogFeeds[index].commentCount = action.payload.comments.length;
            return {
                ...state,
                blogFeeds: tempBlogFeeds
            }
        case BlogActions.TOGGLE_COMMENT_FORM:
            var tempBlogFeeds = [...state.blogFeeds];
            var index = tempBlogFeeds.findIndex(x => x.id == action.payload);
            tempBlogFeeds[index].displayComments = !tempBlogFeeds[index].displayComments;
            return {
                ...state,
                blogFeeds: tempBlogFeeds
            }
        // case BlogActions.SET_LIKED_FEED_NUMBER_LIST:
        //     var tempFeeds = [...state.blogFeeds];
        //     action.payload.forEach((feedId) => {
        //         var index = tempFeeds.findIndex(x => x.id == feedId);
        //         if(index >= 0)
        //         {
        //             tempFeeds[index].isLiked = true;
        //         }
        //     });
        //     return {
        //         ...state,
        //         blogFeeds: tempFeeds,
        //         likedBlogFeedList: action.payload,
        //     };

        case BlogActions.CLEAR_BLOG:
            // localStorage.removeItem('blogs'); 
            return {
                ...state,
                blogs: null
            }
        default:
            return state;
    }
}