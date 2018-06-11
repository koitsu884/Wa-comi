import * as BlogActions from './blogs.actions';
import * as fromApp from '../../store/app.reducer';
import { Blog } from '../../_models/Blog';

export interface State {
    blogs: Blog[];
}

const initialState: State = {
    blogs: []
};

export function blogReducer(state = initialState, action: BlogActions.AccountActions ){
    switch(action.type){
        case BlogActions.SET_BLOG:
            var tempBlog = action.payload == null ? [] : action.payload;
            localStorage.setItem('blogs', JSON.stringify(tempBlog));
            return {
                ...state,
                blogs: tempBlog,
            }
        case BlogActions.ADD_BLOG:
            localStorage.setItem('blogs', JSON.stringify([...state.blogs, action.payload])); 
            return {
                ...state,
                blogs: [...state.blogs, action.payload]
            };
        case BlogActions.DELETE_BLOG:
            const oldBlogs = [...state.blogs];
            var index = oldBlogs.findIndex(x => x.id == action.payload);
            oldBlogs.splice(index, 1);
            localStorage.setItem('blogs', JSON.stringify(oldBlogs)); 
            return{
                ...state,
                blogs: oldBlogs
            };
        case BlogActions.CLEAR_BLOG:
            localStorage.removeItem('blogs'); 
            return{
                ...state,
                blogs:null
            }
        default:
            return state;
    }
}