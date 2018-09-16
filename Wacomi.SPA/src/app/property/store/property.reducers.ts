import * as fromApp from '../../store/app.reducer';
import * as PropertyActions from '../store/property.actions';
import { Property } from '../../_models/Property';
import { PropertySearchOptions } from '../../_models/PropertySearchOptions';
import { Pagination } from '../../_models/Pagination';

export interface FeatureState extends fromApp.AppState {
    property: State
}

export interface State {
    selectedProperty: Property;
    propertiesFound: Property[];
    searchParams: PropertySearchOptions;
    pagination: Pagination
}

const initialState: State = {
    selectedProperty: null,
    propertiesFound: [],
    searchParams: <PropertySearchOptions>{
        categoryIds: [],
        rentTypes: [],
        cityId: 0,
        pet: 0,
        child: 0
    },
    pagination: null
};

export function propertyReducer(state = initialState, action: PropertyActions.PropertyActions) {
    let tempPagination: Pagination;
    switch (action.type) {
        case PropertyActions.SET_PROPERTY:
            return {
                ...state,
                selectedProperty: action.payload,
            }
        case PropertyActions.SET_PROPERTY_SEARCH_OPTIONS:
            return {
                ...state,
                searchParams: action.payload,
            }
        case PropertyActions.SET_PROPERTY_PAGE:
            tempPagination = state.pagination;
            tempPagination.currentPage = action.payload;
            return {
                ...state,
                pagination: tempPagination,
            }
        case PropertyActions.CLEAR_PROPERTY_SEARCH_OPTIONS:
            return {
                ...state,
                searchParams: <PropertySearchOptions>{
                    categoryIds: [],
                    rentTypes: [],
                    cityId: 0,
                    pet: 0,
                    child: 0
                },
                pagination: null
            }
        case PropertyActions.SET_PROPERTY_SEARCH_RESULT:
            //-- Should remove some results here if it is out of circle --
            return {
                ...state,
                propertiesFound: action.payload.properties,
                pagination: action.payload.pagination
            }
        default:
            return state;
    }
}