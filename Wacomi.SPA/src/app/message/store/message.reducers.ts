import * as MessageActions from './message.actions';
import { Message } from '../../_models/Message';

export interface State {
    sentMessages: Message[],
    receivedMessages: Message[],
    sendingMessage: Message,
    messageReplyingTo: string,
    selectedMessage: Message
}

const initialState: State = {
    sentMessages : null,
    receivedMessages : null,
    sendingMessage: null,
    messageReplyingTo : null,
    selectedMessage : null
};

export function messageReducer(state = initialState, action: MessageActions.MessageActions ){
    switch(action.type){
        case MessageActions.SET_MESSAGES_SENT:
            return {
                ...state,
                sentMessages: action.payload,
            }
        case MessageActions.SET_MESSAGES_RECEIVED:
            return {
                ...state,
                receivedMessages: action.payload,
            }
        case MessageActions.SET_BASE_INFORMATION:
            return {
                ...state,
                sendingMessage: action.payload.message,
                messageReplyingTo : action.payload.messageReplyingTo
            }
        case MessageActions.CLEAR_MESSAGE:
            return {
                ...state,
                receivedMessages: null,
                sentMessages: null,
                sendingMessage: null,
                messageReplyingTo: null,
                selectedMessage: null
            }
        case MessageActions.SET_SELECTED_MESSAGE:
            return {
                ...state,
                selectedMessage: action.payload
            }
        default:
            return state;
    }
}