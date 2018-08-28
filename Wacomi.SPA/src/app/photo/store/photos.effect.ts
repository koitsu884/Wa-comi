import { Injectable } from "@angular/core";
import { Effect, Actions } from "@ngrx/effects";
import { AlertifyService } from "../../_services/alertify.service";
import * as fromPhoto from './photos.reducers';
import * as PhotoActions from './photos.action';
import { Store } from "@ngrx/store";
import { of } from "rxjs/observable/of";
import { Photo } from "../../_models/Photo";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";

@Injectable()
export class PhotoEffect{
    baseUrl = environment.apiUrl;
    constructor(private actions$: Actions, 
        private store$: Store<fromPhoto.FeatureState>,
        private httpClient: HttpClient,
        private alertify: AlertifyService){}

    @Effect()
    getPhotos = this.actions$
        .ofType(PhotoActions.GET_PHOTOS)
        .map((action: PhotoActions.GetPhotos) => { return action.payload })
        .switchMap((payload) => {
            return this.httpClient.get<Photo[]>(this.baseUrl + 'photo/' + payload.recordType + "/" + payload.recordId)
            .mergeMap((photos) => {
                return [
                    {
                        type: PhotoActions.SET_PHOTOS,
                        payload: {recordType: payload.recordType, recordId: payload.recordId, photos: photos}
                    }
                ];
            })
            .catch((error) => {
                return of({ type: 'FAILED', payload: "画像の取得に失敗しました: " + error })
            });
        })

    @Effect()
    deletePhoto = this.actions$
        .ofType(PhotoActions.TRY_DELETE_PHOTO)
        .map((actions: PhotoActions.TryDeletePhoto) => {return actions.payload})
        .withLatestFrom(this.store$)
        .switchMap(([id , photoState]) => {
        //.switchMap(payload => {
            this.alertify.message("写真を削除中…");
            return this.httpClient.delete(this.baseUrl + 'photo/' + photoState.photo.recordType + '/' + photoState.photo.recordId + '/' + id)
                .mergeMap(() => {
                    return [
                        {
                            type: 'SUCCESS',
                            payload: "削除しました"
                        },
                        {
                            type: PhotoActions.DELETE_PHOTO,
                            payload: id
                        }
                    ]
                }
                )
                .catch((error: string) => {
                    return of({ type: 'FAILED', payload: error })
                });
        })

    @Effect()
    tryAddPhoto = this.actions$
        .ofType(PhotoActions.TRY_ADD_PHOTO)
        .map((actions: PhotoActions.TryAddPhoto) => {return actions.payload})
        .withLatestFrom(this.store$)
        .switchMap(([fileData , photoState]) => {
            const formData: FormData = new FormData();
     //       formData.append('File', fileData, fileData.name);
            formData.append('files', fileData);
            formData.append('Description', 'Test');

            this.alertify.message("写真をアップロード中…");

            return this.httpClient.post(
                this.baseUrl + 'photo/' + photoState.photo.recordType + '/' + photoState.photo.recordId,
                formData)
            .mergeMap(() => {
                return [
                    {
                        type: 'SUCCESS',
                        payload: "アップロードしました"
                    },
                    {
                        type: PhotoActions.GET_PHOTOS,
                        payload: {recordType:photoState.photo.recordType, recordId: photoState.photo.recordId}
                    }
                ]
            }
            )
            .catch((error: string) => {
                return of({ type: 'FAILED', payload: error })
            });
        })

}