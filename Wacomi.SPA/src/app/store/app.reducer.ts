import { ActionReducerMap } from '@ngrx/store';
import * as fromAccount from '../account/store/account.reducers';
import * as fromPhoto from '../photo/store/photos.reducers';
import * as fromBlog from '../blog/store/blogs.reducers';
import * as fromGlobal from './global.reducers';

export interface AppState {
  global: fromGlobal.State;
  account: fromAccount.State;
  photos: fromPhoto.State;
  blogs: fromBlog.State;
}

export const reducers: ActionReducerMap<AppState> = {
  global: fromGlobal.globalReducer,
  account: fromAccount.accountReducer,
  photos: fromPhoto.photoReducer,
  blogs: fromBlog.blogReducer
};
