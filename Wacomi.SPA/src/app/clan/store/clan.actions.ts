import { Action } from "@ngrx/store";
import { ClanSeek } from "../../_models/ClanSeek";

export const GET_CLANSEEK = 'GET_CLANSEEK';
export const SET_CLANSEEK = 'SET_CLANSEEK';
export const TRY_ADD_CLANSEEK = 'TRY_ADD_CLANSEEK';
export const TRY_DELETE_CLANSEEK = 'TRY_DELETE_CLANSEEK';
export const UPDATE_CLANSEEK = 'UPDATE_CLANSEEK';

export class TryAddClanSeek implements Action {
    readonly type = TRY_ADD_CLANSEEK;
    constructor(public payload: ClanSeek) {}
}

export class GetClanSeek implements Action {
    readonly type = GET_CLANSEEK;
    constructor(public payload: number) {}
}

export class SetClanSeek implements Action {
    readonly type = SET_CLANSEEK;
    constructor(public payload: ClanSeek) {}
}

export class UpdateClanSeek implements Action {
    readonly type = UPDATE_CLANSEEK;

    constructor(public payload: ClanSeek) {}
}

export class TryDeleteClanSeek implements Action {
    readonly type = TRY_DELETE_CLANSEEK;

    constructor(public payload: number) {}
}


export type ClanSeekActions = TryAddClanSeek | GetClanSeek | SetClanSeek | UpdateClanSeek | TryDeleteClanSeek;