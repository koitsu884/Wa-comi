import { Action } from "@ngrx/store";
import { City } from "../_models/City";
import { Hometown } from "../_models/Hometown";
import { KeyValue } from "../_models/KeyValue";
import { Category } from "../_models/Category";

export const GET_CITY_LIST = 'GET_CITY_LIST';
export const SET_CITY_LIST = 'SET_CITY_LIST';
export const GET_HOMETOWN_LIST = 'GET_HOMETOWN_LIST';
export const SET_HOMETOWN_LIST = 'SET_HOMETOWN_LIST';
export const GET_ATTRACTION_CATEGORY_LIST = 'GET_ATTRACTION_CATEGORY_LIST';
export const SET_ATTRACTION_CATEGORY_LIST = 'SET_ATTRACTION_CATEGORY_LIST';
export const GET_CLANCATEGORY_LIST = 'GET_CLANCATEGORY_LIST';
export const SET_CLANCATEGORY_LIST = 'SET_CLANCATEGORY_LIST';
export const TRY_ADD_PHOTOS = 'TRY_ADD_PHOTOS';
export const SUCCESS = 'SUCCESS';
export const FAILED = 'FAILED';



export class GetCityList implements Action {
    readonly type = GET_CITY_LIST;
    constructor() {}
}

export class SetCityList implements Action {
    readonly type = SET_CITY_LIST;
    constructor(public payload: City[]) {}
}

export class GetHometownList implements Action {
    readonly type = GET_HOMETOWN_LIST;
    constructor() {}
}

export class SetHometownList implements Action {
    readonly type = SET_HOMETOWN_LIST;
    constructor(public payload: KeyValue[]) {}
}

export class GetClanCategoryList implements Action {
    readonly type = GET_CLANCATEGORY_LIST;
    constructor() {}
}

export class SetClanCategoryList implements Action {
    readonly type = SET_CLANCATEGORY_LIST;
    constructor(public payload: KeyValue[]) {}
}

export class GetAttractionCategoryList implements Action {
    readonly type = GET_ATTRACTION_CATEGORY_LIST;
    constructor() {}
}

export class SetAttractionCategoryList implements Action {
    readonly type = SET_ATTRACTION_CATEGORY_LIST;
    constructor(public payload: Category[]) {}
}

export class TryAddPhotos implements Action {
    readonly type = TRY_ADD_PHOTOS;
    constructor(public payload: {recordType:string, recordId: number, formData:FormData}){};
}

export class Failed implements Action {
    readonly type = FAILED;

    constructor(public payload: string){}
}

export class Success implements Action {
    readonly type = SUCCESS;

    constructor(public payload: string){}
}

export type GlobalActions = GetCityList 
                          | SetCityList 
                          | GetHometownList 
                          | SetHometownList
                          | GetClanCategoryList
                          | SetClanCategoryList
                          | GetAttractionCategoryList
                          | SetAttractionCategoryList
                          | Success
                          | Failed;