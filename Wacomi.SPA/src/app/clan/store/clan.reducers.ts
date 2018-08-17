import * as ClanSeekActions from './clan.actions';
import { ClanSeek } from '../../_models/ClanSeek';
import * as fromApp from '../../store/app.reducer';
import { Pagination } from '../../_models/Pagination';

export interface FeatureState extends fromApp.AppState {
    clan: State
}

export interface State {
    editingClan: ClanSeek;
    uploading: boolean;
    clanSeeks: ClanSeek[];
    selectedCityId: number;
    selectedCategoryId: number;
    pagination: Pagination,
    loading: boolean;
    reachLimit: boolean;
}

const initialState: State = {
    editingClan: null,
    uploading: false,
    clanSeeks: [],
    selectedCityId: 0,
    selectedCategoryId: 0,
    pagination: null,
    loading: false,
    reachLimit: false
};

export function clanSeekReducer(state = initialState, action: ClanSeekActions.ClanSeekActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case ClanSeekActions.SET_EDITING_CLANSEEK:
            return {
                ...state,
                editingClan: action.payload,
            }
        case ClanSeekActions.SET_CLANSEEK_FILTERS:
            tempPagination = state.pagination;
            if (tempPagination)
                tempPagination.currentPage = 1;
            return {
                ...state,
                selectedCityId: action.payload.cityId,
                selectedCategoryId: action.payload.categoryId,
                pagination: tempPagination
            }
        case ClanSeekActions.CLEAR_CLANSEEK_FILTERS:
            tempPagination = state.pagination;
            if (tempPagination)
                tempPagination.currentPage = 1;
            return {
                ...state,
                selectedCategoryId: 0,
                selectedCityId: 0,
                pagination: tempPagination
            }
        case ClanSeekActions.SET_CLANSEEK_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination
            }
        case ClanSeekActions.SEARCH_CLANSEEKS:
            return {
                ...state,
                clanSeeks: null,
                loading: true
            }
        case ClanSeekActions.SET_CLANSEEK_SEARCH_RESULT:
            return {
                ...state,
                clanSeeks: action.payload.clanSeeks,
                pagination: action.payload.pagination,
                loading: false,
                editingClan: null
            }
        case ClanSeekActions.SET_COUNTLIMIT_FLAG:
            return {
                ...state,
                reachLimit: action.payload
            }
        case ClanSeekActions.TRY_ADD_CLANSEEK_PHOTOS:
            return {
                ...state,
                uploading: true
            }
        default:
            return state;
    }
}