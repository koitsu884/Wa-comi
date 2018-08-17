import { Action } from "@ngrx/store";
import { ClanSeek } from "../../_models/ClanSeek";
import { Pagination } from "../../_models/Pagination";
import { Photo } from "../../_models/Photo";

export const GET_CLANSEEK = 'GET_CLANSEEK';
export const CHECK_CLANSEEKS_COUNTLIMIT = 'CHECK_CLANSEEKS_COUNTLIMIT';
export const SET_COUNTLIMIT_FLAG = 'SET_COUNTLIMIT_FLAG';
export const SET_EDITING_CLANSEEK = 'SET_EDITING_CLANSEEK';
export const SET_CLANSEEK_FILTERS = 'SET_CLANSEEK_FILTERS';
export const SET_CLANSEEK_PAGE = 'SET_CLANSEEK_PAGE';
export const CLEAR_CLANSEEK_FILTERS = 'CLEAR_CLANSEEK_FILTERS';
export const SEARCH_CLANSEEKS = 'SEARCH_CLANSEEKS';
export const SET_CLANSEEK_SEARCH_RESULT = 'SET_CLANSEEK_SEARCH_RESULT';
export const TRY_ADD_CLANSEEK = 'TRY_ADD_CLANSEEK';
export const TRY_ADD_CLANSEEK_PHOTOS = 'TRY_ADD_CLANSEEK_PHOTOS';
export const TRY_DELETE_CLANSEEK = 'TRY_DELETE_CLANSEEK';
export const UPDATE_CLANSEEK = 'UPDATE_CLANSEEK';

export class TryAddClanSeek implements Action {
    readonly type = TRY_ADD_CLANSEEK;
    constructor(public payload: { clanSeek: ClanSeek, formData:FormData}) {}
}

export class TryAddClanSeekPhotos implements Action {
    readonly type = TRY_ADD_CLANSEEK_PHOTOS;
    constructor(public payload: {clanSeekId: number, formData:FormData}){}
}

export class GetClanSeek implements Action {
    readonly type = GET_CLANSEEK;
    constructor(public payload: number) {}
}

export class CheckClanseeksCountLimit implements Action {
    readonly type = CHECK_CLANSEEKS_COUNTLIMIT;
    constructor(public payload: number) {} //appUserId
}

export class SetCountLimitFlag implements Action {
    readonly type = SET_COUNTLIMIT_FLAG;
    constructor(public payload: boolean){}
}

export class SetClanSeek implements Action {
    readonly type = SET_EDITING_CLANSEEK;
    constructor(public payload: ClanSeek) {}
}

export class SetClanSeekFilters implements Action {
    readonly type = SET_CLANSEEK_FILTERS;
    constructor(public payload: {cityId: number, categoryId: number}){}
}

export class SetClanSeekPage implements Action {
    readonly type = SET_CLANSEEK_PAGE;
    constructor(public payload: number){}
}

export class ClearClanseekFilters implements Action {
    readonly type = CLEAR_CLANSEEK_FILTERS;
    constructor(){};
}

export class SearchClanSeeks implements Action {
    readonly type =SEARCH_CLANSEEKS;
    constructor(){}
}

export class SetClanSeekSearchResult implements Action {
    readonly type = SET_CLANSEEK_SEARCH_RESULT;
    constructor(public payload: {clanSeeks: ClanSeek[], pagination: Pagination}) {}
}

export class UpdateClanSeek implements Action {
    readonly type = UPDATE_CLANSEEK;

    constructor(public payload: ClanSeek) {}
}

export class TryDeleteClanSeek implements Action {
    readonly type = TRY_DELETE_CLANSEEK;

    constructor(public payload: number) {}
}


export type ClanSeekActions = TryAddClanSeek 
                            | TryAddClanSeekPhotos
                            | GetClanSeek 
                            | CheckClanseeksCountLimit
                            | SetCountLimitFlag
                            | SetClanSeek 
                            | SetClanSeekFilters
                            | SetClanSeekPage
                            | ClearClanseekFilters
                            | SearchClanSeeks
                            | SetClanSeekSearchResult
                            | UpdateClanSeek 
                            | TryDeleteClanSeek;