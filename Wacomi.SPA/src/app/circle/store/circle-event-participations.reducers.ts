import { CircleEventParticipation } from "../../_models/CircleEventParticipation";
import * as CircleEventParticipationsActions from '../store/circle-event-participations.actions';
import { Pagination } from "../../_models/Pagination";

export interface State {
    eventParticipants: CircleEventParticipation[];
    numberOfParticipants: number;
    pagination: Pagination;
    eventParticipantsWaiting: CircleEventParticipation[];
    numberOfWaiting: number;
    waitingPagination: Pagination;
    eventParticipantsCanceled: CircleEventParticipation[];
    numberOfCanceld: number;
    canceledPagination: Pagination;
}

const initialState: State = {
    eventParticipants: [],
    numberOfParticipants: 0,
    pagination: null,
    eventParticipantsWaiting: [],
    numberOfWaiting: 0,
    waitingPagination: null,
    eventParticipantsCanceled: [],
    numberOfCanceld: 0,
    canceledPagination: null
}

export function circleEventParticipationReducer(state = initialState, action: CircleEventParticipationsActions.CircleEventParticipationsActions) {
    switch (action.type) {
        case CircleEventParticipationsActions.GET_CIRCLE_EVENT_CONFIRMEDLIST:
            return {
                ...state,
                eventParticipants: [],
                numberOfParticipants: 0
            }
        case CircleEventParticipationsActions.SET_CIRCLE_EVENT_CONFIRMEDLIST:
            return {
                ...state,
                eventParticipants: action.payload.participants,
                pagination: action.payload.pagination,
                numberOfParticipants: action.payload.pagination.totalItems
            }
        case CircleEventParticipationsActions.GET_CIRCLE_EVENT_WAITLIST:
            return {
                ...state,
                eventParticipantsWaiting: [],
                numberOfWaiting:0
            }
        case CircleEventParticipationsActions.SET_CIRCLE_EVENT_WAITLIST:
            return {
                ...state,
                eventParticipantsWaiting: action.payload.participants,
                waitingPagination: action.payload.pagination,
                numberOfWaiting: action.payload.pagination.totalItems
            }
        case CircleEventParticipationsActions.GET_CIRCLE_EVENT_CANCELEDLIST:
            return {
                ...state,
                eventParticipantsCanceled: [],
                numberOfCanceld:0
            }
        case CircleEventParticipationsActions.SET_CIRCLE_EVENT_CANCELEDLIST:
            return {
                ...state,
                eventParticipantsCanceled: action.payload.participants,
                canceledPagination: action.payload.pagination,
                numberOfCanceld: action.payload.pagination.totalItems
            }
        case CircleEventParticipationsActions.APPROVE_EVENT_PARTICIPATION_REQUEST:
            const tempEventParticipationsWaiting = [...state.eventParticipantsWaiting];
            const tempEventParticipations = [...state.eventParticipants];
            var index = tempEventParticipationsWaiting.findIndex(ep => ep.appUserId == action.payload.appUserId);
            tempEventParticipations.unshift(tempEventParticipationsWaiting[index]);
            tempEventParticipationsWaiting.splice(index, 1);

            return {
                ...state,
                eventParticipants: tempEventParticipations, 
                eventParticipantsWaiting: tempEventParticipationsWaiting,
                numberOfParticipants: state.numberOfParticipants + 1,
                numberOfWaiting: state.numberOfWaiting - 1
            }
        default:
            return state;
    }
}