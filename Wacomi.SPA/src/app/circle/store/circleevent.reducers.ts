import * as CircleEventActions from '../store/circleevent.actions';
import { CircleEvent } from '../../_models/CircleEvent';
import { CircleEventParticipation, CircleEventParticipationStatus } from '../../_models/CircleEventParticipation';

export interface State {
    eventList: CircleEvent[];
    //Search params
    circleId: number;
    categoryId: number;
    fromDate: Date;
    cityId :number;
    lastPageNumber: number;
    finish: boolean;
    reload: boolean;
    //For Detail & Edit
    selectedCircleEvent: CircleEvent;
    latestEventParticipants: CircleEventParticipation[];
}

const initialState: State = {
    eventList: [],
    lastPageNumber : 1,
    circleId: null,
    categoryId: null,
    fromDate:null,
    cityId:null,
    finish: false,
    reload: false,
    selectedCircleEvent: null,
    latestEventParticipants: []
};

export function circleEventReducer(state = initialState, action: CircleEventActions.CircleEventActions) {
    switch (action.type) {
        case CircleEventActions.SET_CIRCLE_EVENT_SEARCH_PARAMS:
            return {
                ...state,
                circleId : action.payload.circleId,
                categoryId : action.payload.categoryId,
                cityId: action.payload.cityId,
                fromDate : action.payload.fromDate,
                eventList: [],
                finish: false,
                reload: false,
                lastPageNumber: 1
            }
        case CircleEventActions.GET_CIRCLE_EVENT:
            return {
                ...state,
                selectedCircleEvent: null
            }
        case CircleEventActions.SET_CIRCLE_EVENT:
            return {
                ...state,
                selectedCircleEvent: action.payload
            }
        case CircleEventActions.LOAD_NEXT_CIRCLE_EVENT_LIST:
            return {
                ...state,
                lastPageNumber: state.finish ? state.lastPageNumber : state.lastPageNumber + 1
            }
        case CircleEventActions.SET_EVENT_LOAD_FINISH_FLAG:
            return {
                ...state,
                finish: true
            }
        case CircleEventActions.SET_EVENT_RELOAD_FLAG:
            return {
                ...state,
                reload: true
            }
        case CircleEventActions.ADD_CIRCLE_EVENT_LIST:
            return {
                ...state,
                eventList : state.eventList.concat(action.payload)
            }
        case CircleEventActions.SET_EVENT_PARTICIPATION_STATUS:
            const tempSelectedEvent = state.selectedCircleEvent;
            tempSelectedEvent.myStatus = action.payload;
            if(action.payload == CircleEventParticipationStatus.Confirmed)
                tempSelectedEvent.numberOfPaticipants++;
            return {
                ...state,
                selectedCircleEvent: tempSelectedEvent
            }
        case CircleEventActions.GET_LATEST_EVENT_PARTICIPANTS:
            return {
                ...state,
                latestEventParticipants: []
            }
        case CircleEventActions.SET_LATEST_EVENT_PARTICIPANTS:
            return {
                ...state,
                latestEventParticipants: action.payload
            }
        case CircleEventActions.DELETE_EVENT_PARTICIPATION:
            const tempEvent = state.selectedCircleEvent;
            tempEvent.myStatus = null;
            return {
                ...state,
                selectedCircleEvent: tempEvent
            }
        case CircleEventActions.CANCEL_EVENT_PARTICIPATION:
            const tempEvent2 = state.selectedCircleEvent;
            tempEvent2.myStatus = CircleEventParticipationStatus.Canceled;
            tempEvent2.numberOfPaticipants--;
            tempEvent2.numberOfCanceled++;
            return {
                ...state,
                selectedCircleEvent: tempEvent2
            }
        default:
            return state;
    }
}