import { Action } from "@ngrx/store";
import { City } from "../_models/City";
import { KeyValue } from "../_models/KeyValue";
import { Category } from "../_models/Category";

export const GET_CITY_LIST = 'GET_CITY_LIST';
export const SET_CITY_LIST = 'SET_CITY_LIST';
export const GET_HOMETOWN_LIST = 'GET_HOMETOWN_LIST';
export const SET_HOMETOWN_LIST = 'SET_HOMETOWN_LIST';
export const GET_ATTRACTION_CATEGORY_LIST = 'GET_ATTRACTION_CATEGORY_LIST';
export const SET_ATTRACTION_CATEGORY_LIST = 'SET_ATTRACTION_CATEGORY_LIST';
export const GET_PROPERTY_CATEGORY_LIST = 'GET_PROPERTY_CATEGORY_LIST';
export const SET_PROPERTY_CATEGORY_LIST = 'SET_PROPERTY_CATEGORY_LIST';
export const GET_CIRCLE_CATEGORY_LIST = 'GET_CIRCLE_CATEGORY_LIST';
export const SET_CIRCLE_CATEGORY_LIST = 'SET_CIRCLE_CATEGORY_LIST';
export const GET_CLANCATEGORY_LIST = 'GET_CLANCATEGORY_LIST';
export const SET_CLANCATEGORY_LIST = 'SET_CLANCATEGORY_LIST';
export const TRY_ADD_PHOTOS = 'TRY_ADD_PHOTOS';
export const TRY_ADD_RECORD = 'TRY_ADD_RECORD';
export const GET_RECORD = 'GET_RECORD';
export const DELETE_RECORD = 'DELETE_RECORD';
export const UPDATE_RECORD = 'UPDATE_RECORD';
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

export class GetPropertyCategoryList implements Action {
    readonly type = GET_PROPERTY_CATEGORY_LIST;
    constructor() {}
}

export class SetPropertyCategoryList implements Action {
    readonly type = SET_PROPERTY_CATEGORY_LIST;
    constructor(public payload: Category[]) {}
}

export class GetCircleCategoryList implements Action {
    readonly type = GET_CIRCLE_CATEGORY_LIST;
    constructor() {}
}

export class SetCircleCategoryList implements Action {
    readonly type = SET_CIRCLE_CATEGORY_LIST;
    constructor(public payload: Category[]) {}
}

export class TryAddPhotos implements Action {
    readonly type = TRY_ADD_PHOTOS;
    constructor(public payload: {recordType:string, recordId: number, formData:FormData, callbackLocation:string, callbackActions?: {type:string, payload:any}[]}){};
}

export class GetRecord implements Action {
    readonly type = GET_RECORD;
    constructor(public payload: {recordType:string, recordId:number, callbackAction:string}){};
}

export class DeleteRecord implements Action {
    readonly type = DELETE_RECORD;
    constructor(public payload: {recordType:string, recordId:number, callbackLocation?:string, callbackActions?: {type:string, payload:any}[]}){};
}

export class TryAddRecord implements Action {
    readonly type = TRY_ADD_RECORD;
    constructor(public payload: {recordType:string, record: any, formData?:FormData, callbackLocation?:string, callbackActions?: {type:string, payload:any}[]}){};
}

export class UpdateRecord implements Action {
    readonly type = UPDATE_RECORD;
    constructor(public payload: {recordType:string, record: any, callbackLocation?:string, recordSetActionType?:string}){};
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
                          | GetPropertyCategoryList
                          | SetPropertyCategoryList
                          | GetCircleCategoryList
                          | SetCircleCategoryList
                          | TryAddRecord
                          | UpdateRecord
                          | DeleteRecord
                          | Success
                          | Failed;