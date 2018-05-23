import { Photo } from '../../_models/Photo';
import * as PhotoActions from './photos.action';
import * as fromApp from '../../store/app.reducer';

export interface State {
    type: string;
    recordId: number;
    photos: Photo[];
}

const initialState: State = {
    type : null,
    recordId: null,
    photos: []
};

export function photoReducer(state = initialState, action: PhotoActions.PhotoActions ){
    switch(action.type){
        case PhotoActions.SET_PHOTOS:
            localStorage.setItem('photos', JSON.stringify(action.payload.photos));
            return {
                ...state,
                photos: [...action.payload.photos],
                type: action.payload.type,
                recordId: action.payload.recordId
            }
        case PhotoActions.ADD_PHOTO:
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