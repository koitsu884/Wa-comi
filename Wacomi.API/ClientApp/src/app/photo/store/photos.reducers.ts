import { Photo } from '../../_models/Photo';
import * as PhotoActions from './photos.action';
import * as fromApp from '../../store/app.reducer';

export interface FeatureState extends fromApp.AppState {
    photo : State
}

export interface State {
    // appUserId: number;
    recordType: string;
    recordId: number;
    photos: Photo[];
}

const initialState: State = {
    //  appUserId: null,
    recordType: null,
    recordId: null,
    photos: []
};

export function photoReducer(state = initialState, action: PhotoActions.PhotoActions) {
    switch (action.type) {
        // case PhotoActions.SET_USER_ID:
        //     return{
        //         ...state,
        //         appUserId : action.payload
        //     }
        case PhotoActions.SET_PHOTOS:
            return {
                ...state,
                recordType: action.payload.recordType,
                recordId: action.payload.recordId,
                photos: [...action.payload.photos]
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
            return {
                ...state,
                photos: oldPhotos
            };
        case PhotoActions.CLEAR_PHOTO:
            return {
                ...state,
                recordType: null,
                recordId: null,
                photos: null
            }
        default:
            return state;
    }
}