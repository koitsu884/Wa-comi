import { Action } from "@ngrx/store";
import { Photo } from "../../_models/Photo";

export const GET_PHOTOS = 'GET_PHOTOS';
export const SET_PHOTOS = 'SET_PHOTOS';
export const SET_USER_ID = 'SET_USER_ID';
export const ADD_PHOTO = 'ADD_PHOTO';
export const DELETE_PHOTO = 'DELETE_PHOTO';
export const TRY_ADD_PHOTO = 'TRY_ADD_PHOTO';
export const TRY_DELETE_PHOTO = 'TRY_DELETE_PHOTO';
export const CLEAR_PHOTO ='CLEAR_PHOTO';

export class GetPhotos implements Action{
    readonly type = GET_PHOTOS;

    constructor(public payload: number){} //appUserId
}

export class SetPhotos implements Action{
    readonly type = SET_PHOTOS;

    constructor(public payload:  Photo[]){}
}

export class SetUserId implements Action{
    readonly type = SET_USER_ID;

    constructor(public payload:  number){}
}

export class TryAddPhoto implements Action{
    readonly type = TRY_ADD_PHOTO;

    constructor(public payload: {appUserId :number,  fileData:File}){}
}

export class TryDeletePhoto implements Action{
    readonly type = TRY_DELETE_PHOTO;

    constructor(public payload: {userId: number, id: number}){}
}

export class AddPhoto implements Action{
    readonly type = ADD_PHOTO;

    constructor(public payload: Photo){}
}

export class DeletePhoto implements Action{
    readonly type = DELETE_PHOTO;

    constructor(public payload: number){}
}

export class ClearPhoto implements Action {
    readonly type = CLEAR_PHOTO;

    constructor() {}
}

export type PhotoActions = SetPhotos 
                        | SetUserId
                        | GetPhotos
                        | TryAddPhoto 
                        | TryDeletePhoto 
                        | AddPhoto 
                        | DeletePhoto
                        | ClearPhoto;