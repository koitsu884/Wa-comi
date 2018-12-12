import * as MessageActions from './message.actions';
import { Message } from '../../_models/Message';
import { Pagination } from '../../_models/Pagination';

export interface State {
    // sentMessages: Message[],
    // receivedMessages: Message[],
    messages: Message[],
    pagination: Pagination,
    sendingMessage: Message,
    messageReplyingTo: string,
    selectedMessage: Message
}

const initialState: State = {
    // sentMessages : null,
    // receivedMessages : null,
    messages: null,
    pagination: null,
    sendingMessage: null,
    messageReplyingTo : null,
    selectedMessage : null
};

export function messageReducer(state = initialState, action: MessageActions.MessageActions ){
    switch(action.type){
        case MessageActions.SET_MESSAGES:
            return {
                ...state,
                messages: action.payload.messages,
                pagination: action.payload.pagination
            }
        // case MessageActions.SET_MESSAGES_SENT:
        //     return {
        //         ...state,
        //         sentMessages: action.payload.messages,
        //         pagination: action.payload.pagination
        //     }
        // case MessageActions.SET_MESSAGES_RECEIVED:
        //     return {
        //         ...state,
        //         receivedMessages: action.payload.messages,
        //         patination: action.payload.pagination
        //     }
        case MessageActions.SET_BASE_INFORMATION:
            return {
                ...state,
                sendingMessage: action.payload.message,
                messageReplyingTo : action.payload.messageReplyingTo
            }
        case MessageActions.CLEAR_MESSAGE:
            return {
                ...state,
                messages: null,
                // receivedMessages: null,
                // sentMessages: null,
                sendingMessage: null,
                messageReplyingTo: null,
                selectedMessage: null,
                pagination: null
            }
        case MessageActions.SET_SELECTED_MESSAGE:
            return {
                ...state,
                selectedMessage: action.payload
            }
        case MessageActions.SET_ISREAD_FLAG:
            const tempMessages = [...state.messages];
            var index = tempMessages.findIndex(m => m.id == action.payload);
            tempMessages[index].isRead = true;
            return {
                ...state,
                messages: tempMessages
            }
        default:
            return state;
    }
}