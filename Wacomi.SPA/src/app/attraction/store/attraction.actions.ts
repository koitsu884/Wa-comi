import { Attraction } from "../../_models/Attraction";
import { Action } from "@ngrx/store";
import { Pagination } from "../../_models/Pagination";
import { AttractionEdit } from "../../_models/AttractionEdit";

export const GET_ATTRACTION = 'GET_ATTRACTION';
export const SET_ATTRACTION = 'SET_ATTRACTION';
//============= Attraction home & details (search) ======================
export const SET_ATTRACTION_CITY = 'SET_ATTRACTION_CITY';
export const SET_ATTRACTION_SEARCH_CATEGORIES = 'SET_SEARCH_CATEGORIES';
export const SEARCH_ATTRACTION = 'SEARCH_ATTRACTION';
export const SET_ATTRACTION_SEARCH_RESULT = 'SET_ATTRACTION_SEARCH_RESULT';
export const CLEAR_ATTRACTION_FILTER = 'CLEAR_ATTRACTION_FILTER';

//============= Attraction edit =====================
export const TRY_ADD_ATTRACTION = 'TRY_ADD_ATTRACTION';
export const UPDATE_ATTRACTION = 'UPDATE_ATTRACTION';
export const TRY_DELETE_ATTRACTION = 'TRY_DELETE_ATTRACTION';

export class GetAttraction implements Action {
    readonly type = GET_ATTRACTION;
    constructor(public payload: number) {}
}

export class SetAttraction implements Action {
    readonly type = SET_ATTRACTION;
    constructor(public payload: Attraction) {}
}

export class SetAttractionCity implements Action {
    readonly type = SET_ATTRACTION_CITY;
    constructor(public payload: number) {} //cityId
}

export class SetAttractionSearchCategories implements Action {
    readonly type = SET_ATTRACTION_SEARCH_CATEGORIES;
    constructor(public payload: number[]) {}
}

export class SearchAttraction implements Action {
    readonly type = SEARCH_ATTRACTION;
    constructor(public payload:{categories: number[], cityId: number, appUserId: number}) {}
}

export class SetAttractionSearchResult implements Action {
    readonly type = SET_ATTRACTION_SEARCH_RESULT;
    constructor(public payload:{attractions: Attraction[], pagination: Pagination}) {}
}

export class ClearAttractionFilter implements Action {
    readonly type = CLEAR_ATTRACTION_FILTER;
    constructor(){}
}

export class TryAddAttraction implements Action {
    readonly type = TRY_ADD_ATTRACTION;
    constructor(public payload: { attraction: AttractionEdit, formData:FormData}) {}
}

export class UpdateAttraction implements Action {
    readonly type = UPDATE_ATTRACTION;
    constructor(public payload: AttractionEdit){}
}

export class TryDeleteAttraction implements Action {
    readonly type = TRY_DELETE_ATTRACTION;
    constructor(public payload: number) {}
}




export type AttractionActions = TryAddAttraction
                              | UpdateAttraction
                              | GetAttraction
                              | SetAttraction
                              | SetAttractionCity
                              | ClearAttractionFilter
                              | SetAttractionSearchCategories
                              | SearchAttraction
                              | SetAttractionSearchResult
                              | TryDeleteAttraction;