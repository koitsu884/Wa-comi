import * as BlogActions from './blogs.actions';
import * as fromApp from '../../store/app.reducer';
import { Blog } from '../../_models/Blog';

export interface State {
    blogs: Blog[];
}

const initialState: State = {
    blogs: null
};

export function blogReducer(state = initialState, action: BlogActions.AccountActions ){
    switch(action.type){
        case BlogActions.SET_BLOG:
            return {
                ...state,
                blogs: [...action.payload],
            }
        case BlogActions.ADD_BLOG:
            return {
                ...state,
                blogs: [...state.blogs, action.payload]
            };
        case BlogActions.DELETE_BLOG:
            const oldBlogs = [...state.blogs];
            var index = oldBlogs.findIndex(x => x.id == action.payload);
            oldBlogs.splice(index, 1);
            return{
                ...state,
                blogs: oldBlogs
            };
        default:
            return state;
    }
}