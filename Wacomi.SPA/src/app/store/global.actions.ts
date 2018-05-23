import { Action } from "@ngrx/store";
import { City } from "../_models/City";
import { Hometown } from "../_models/Hometown";
import { KeyValue } from "../_models/KeyValue";

export const GET_CITY_LIST = 'GET_CITY_LIST';
export const SET_CITY_LIST = 'SET_CITY_LIST';
export const GET_HOMETOWN_LIST = 'GET_HOMETOWN_LIST';
export const SET_HOMETOWN_LIST = 'SET_HOMETOWN_LIST';
export const GET_CLANCATEGORY_LIST = 'GET_CLANCATEGORY_LIST';
export const SET_CLANCATEGORY_LIST = 'SET_CLANCATEGORY_LIST';
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
                          | Success
                          | Failed;