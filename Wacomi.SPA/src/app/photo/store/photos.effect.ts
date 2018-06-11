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
        private httpClient: HttpClient,
        private alertify: AlertifyService, 
        private store: Store<fromPhoto.State>){}

    @Effect()
    getPhotos = this.actions$
        .ofType(PhotoActions.GET_PHOTOS)
        .map((action: PhotoActions.GetPhotos) => { return action.payload })
        .switchMap((appUserId) => {
            return this.httpClient.get<Photo[]>(this.baseUrl + 'photo/user/' + appUserId)
            .mergeMap((photos) => {
                return [
                    {
                        type: PhotoActions.SET_PHOTOS,
                        payload: photos
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
        .switchMap(payload => {
            this.alertify.message("写真を削除中…");
            return this.httpClient.delete(this.baseUrl + 'photo/' + payload.userId + '/' + payload.id)
                .mergeMap(() => {
                    return [
                        {
                            type: 'SUCCESS',
                            payload: "削除しました"
                        },
                        {
                            type: PhotoActions.DELETE_PHOTO,
                            payload: payload.id
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
        .switchMap(payload => {
            // console.log(payload);
            const formData: FormData = new FormData();
            formData.append('File', payload.fileData, payload.fileData.name);
            formData.append('Description', 'Test');

            this.alertify.message("写真をアップロード中…");

            return this.httpClient.post(this.baseUrl + 'photo/' + payload.appUserId,
            formData)
            .mergeMap((newPhoto: Photo) => {
                return [
                    {
                        type: 'SUCCESS',
                        payload: "アップロードしました"
                    },
                    {
                        type: PhotoActions.ADD_PHOTO,
                        payload: newPhoto
                    }
                ]
            }
            )
            .catch((error: string) => {
                return of({ type: 'FAILED', payload: error })
            });
        })

}