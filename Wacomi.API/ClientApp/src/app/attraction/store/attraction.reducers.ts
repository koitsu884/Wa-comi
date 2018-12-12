import * as fromApp from '../../store/app.reducer';
import * as AttractionActions from './attraction.actions';
import { Attraction } from '../../_models/Attraction';
import { Pagination } from '../../_models/Pagination';
import { AttractionReview } from '../../_models/AttractionReview';

export interface FeatureState extends fromApp.AppState {
    attraction: State
}

export interface State {
    selectedAttraction: Attraction;
    selectedAttractionReview: AttractionReview;
    attractions: Attraction[];
    attractionReviewList: AttractionReview[];
    selectedCityId: number;
    selectedCategories: number[];
    pagination: Pagination,
    loading: boolean;
}

const initialState: State = {
    selectedAttraction: null,
    selectedAttractionReview: null,
    attractions: [],
    attractionReviewList: [],
    selectedCityId: null,
    selectedCategories: [],
    pagination: null,
    loading: false
};

export function attractionReducer(state = initialState, action: AttractionActions.AttractionActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case AttractionActions.GET_ATTRACTION:
            return {
                ...state,
                selectedAttraction: null,
            }
        case AttractionActions.SET_ATTRACTION:
            return {
                ...state,
                selectedAttraction: action.payload,
            }
        case AttractionActions.LIKE_ATTRACTION:
            var temp = state.selectedAttraction;
            temp.isLiked = true;
            temp.likedCount++;
            return {
                ...state,
                selectedAttraction: temp,
            }
        case AttractionActions.SET_ATTRACTION_CITY:
            return {
                ...state,
                selectedCityId: action.payload
            }
        case AttractionActions.SET_ATTRACTION_SEARCH_CATEGORIES:
            tempPagination = state.pagination;
            if (tempPagination)
                tempPagination.currentPage = 1;
            return {
                ...state,
                selectedCategories: action.payload,
               // pagination: tempPagination
            }
        case AttractionActions.CLEAR_ATTRACTION_FILTER:
            return {
                ...state,
                selectedCategories: [],
                selectedCityId: 0
            }
        // case ClanSeekActions.SET_CLANSEEK_PAGE:
        //     tempPagination = state.pagination;
        //     tempPagination.currentPage = action.payload;
        //     return {
        //         ...state,
        //         pagination: tempPagination
        //     }
        case AttractionActions.SEARCH_ATTRACTION:
            return {
                ...state,
                attractions: null,
                pagination: null,
                loading: true
            }
        case AttractionActions.SET_ATTRACTION_SEARCH_RESULT:
            return {
                ...state,
                attractions: action.payload.attractions,
                pagination: action.payload.pagination,
                loading: false,
                selectedAttraction: null
            }
        case AttractionActions.GET_ATTRACTION_REVIEW:
            return {
                ...state,
                selectedAttractionReview: null,
            }
        case AttractionActions.SET_ATTRACTION_REVIEW:
            return {
                ...state,
                selectedAttractionReview: action.payload,
            }
        case AttractionActions.GET_ATTRACTION_REVIEW_LIST:
            return {
                ...state,
                pagination: null,
                loading: true,
                attractionReviewList: []
            }
        case AttractionActions.SET_ATTRACTION_REVIEW_LIST:
            return {
                ...state,
                pagination: action.payload.pagination,
                loading: false,
                attractionReviewList: action.payload.attractionReviewList
            }
        case AttractionActions.LIKE_ATTRACTION_REVIEW:
            var tempReviews = [...state.attractionReviewList];
            var index = tempReviews.findIndex(r => r.id == action.payload.attractionReviewId);
            tempReviews[index].isLiked = true;
            tempReviews[index].likedCount++;
            return {
                ...state,
                attractionReviewList: tempReviews,
            }
        default:
            return state;
    }
}