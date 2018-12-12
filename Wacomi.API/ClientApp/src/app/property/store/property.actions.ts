import { Action } from "@ngrx/store";
import { Property } from "../../_models/Property";
import { PropertyEdit } from "../../_models/PropertyEdit";
import { PropertySearchOptions } from "../../_models/PropertySearchOptions";
import { Pagination } from "../../_models/Pagination";

export const GET_PROPERTY = 'GET_PROPERTY';
export const SET_PROPERTY = 'SET_PROPERTY';
//============= Property edit =====================
export const TRY_ADD_PROPERTY = 'TRY_ADD_PROPERTY';
export const UPDATE_PROPERTY = 'UPDATE_PROPERTY';
export const TRY_DELETE_PROPERTY = 'TRY_DELETE_PROPERTY';
//============= Property home & details (search) ======================
export const SET_PROPERTY_SEARCH_OPTIONS = 'SET_PROPERTY_SEARCH_OPTIONS';
export const SET_PROPERTY_PAGE = 'SET_PROPERTY_PAGE';
export const SEARCH_PROPERTIES = 'SEARCH_PROPERTIES';
export const SET_PROPERTY_SEARCH_RESULT = 'SET_PROPERTY_SEARCH_RESULT';
export const CLEAR_PROPERTY_SEARCH_OPTIONS = 'CLEAR_PROPERTY_SEARCH_OPTIONS';



export class GetProperty implements Action {
    readonly type = GET_PROPERTY;

    constructor(public payload: number) {}
}

export class SetProperty implements Action {
    readonly type = SET_PROPERTY;

    constructor(public payload: Property) {}
}

export class TryAddProperty implements Action {
    readonly type = TRY_ADD_PROPERTY;

    constructor(public payload: {property:PropertyEdit,  formData:FormData}){}
}

export class UpdateProperty implements Action {
    readonly type = UPDATE_PROPERTY;

    constructor(public payload: PropertyEdit) {}
}

export class TryDeleteProperty implements Action {
    readonly type = TRY_DELETE_PROPERTY;

    constructor(public payload: number) {}
}

export class ClearPropertySearchOptions implements Action {
    readonly type = CLEAR_PROPERTY_SEARCH_OPTIONS;

    constructor(){}
}

export class SetPropertySearchOptions implements Action {
    readonly type = SET_PROPERTY_SEARCH_OPTIONS;

    constructor(public payload: PropertySearchOptions){}
}

export class SetPropertyPage implements Action {
    readonly type = SET_PROPERTY_PAGE;

    constructor(public payload: number){}
}

export class SearchProperties implements Action {
    readonly type = SEARCH_PROPERTIES;

    constructor(){}
}

export class SetPropertySearchResult implements Action {
    readonly type = SET_PROPERTY_SEARCH_RESULT;

    constructor(public payload: {properties: Property[], pagination: Pagination}){}
}


export type PropertyActions = GetProperty
                            | SetProperty
                            | TryAddProperty
                            | UpdateProperty
                            | TryDeleteProperty
                            | ClearPropertySearchOptions
                            | SetPropertySearchOptions
                            | SetPropertyPage
                            | SearchProperties
                            | SetPropertySearchResult; 