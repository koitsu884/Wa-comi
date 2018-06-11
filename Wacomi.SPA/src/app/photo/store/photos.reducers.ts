import { Photo } from '../../_models/Photo';
import * as PhotoActions from './photos.action';
import * as fromApp from '../../store/app.reducer';

export interface State {
   // appUserId: number;
    photos: Photo[];
}

const initialState: State = {
  //  appUserId: null,
    photos: []
};

export function photoReducer(state = initialState, action: PhotoActions.PhotoActions ){
    switch(action.type){
        // case PhotoActions.SET_USER_ID:
        //     return{
        //         ...state,
        //         appUserId : action.payload
        //     }
        case PhotoActions.SET_PHOTOS:
            const tempPhotos = action.payload ? action.payload : [];
            localStorage.setItem('photos', JSON.stringify(tempPhotos));
            return {
                ...state,
                photos: [...tempPhotos]
            }
        case PhotoActions.ADD_PHOTO:
            localStorage.setItem('photos', JSON.stringify([...state.photos, action.payload])); 
            return {
                ...state,
                photos: [...state.photos, action.payload]
            };
        case PhotoActions.DELETE_PHOTO:
            const oldPhotos = [...state.photos];
            var index = oldPhotos.findIndex(x => x.id == action.payload);
            oldPhotos.splice(index, 1);
            localStorage.setItem('photos', JSON.stringify(oldPhotos));
            return{
                ...state,
                photos: oldPhotos
            };
        case PhotoActions.CLEAR_PHOTO:
            localStorage.removeItem('photos');
            return{
                ...state,
                photos: null
            }
        default:
            return state;
    }
}