import { Action } from "@ngrx/store";
import { Message } from "../../_models/Message";
import { Pagination } from "../../_models/Pagination";

export const GET_MESSAGES = 'GET_MESSAGES';
export const SET_MESSAGES = 'SET_MESSAGES';

// export const GET_MESSAGES_SENT = 'GET_MESSAGES_SENT';
// export const SET_MESSAGES_SENT = 'SET_MESSAGES_SENT';
// export const GET_MESSAGES_RECEIVED = 'GET_MESSAGES_RECEIVED';
// export const SET_MESSAGES_RECEIVED = 'SET_MESSAGES_RECEIVED';
export const SET_BASE_INFORMATION = 'SET_BASE_INFORMATION';
export const SEND_MESSAGE = 'SEND_MESSAGE';
export const CLEAR_MESSAGE = 'CLEAR_MESSAGE';
export const SET_SELECTED_MESSAGE = 'SET_SELECTED_MESSAGE';
export const SET_ISREAD_FLAG = 'SET_ISREAD_FLAG';

export class GetMessages implements Action {
    readonly type = GET_MESSAGES;
    constructor(public payload: {appUserId: number, type:string, pageNumber?: number, pageSize?: number}) {} 
}

export class SetMessages implements Action {
    readonly type = SET_MESSAGES;
    constructor(public payload: { messages: Message[], pagination: Pagination}) {}
}

// export class GetMessagesSent implements Action {
//     readonly type = GET_MESSAGES_SENT;
//     constructor(public payload: {appUserId: number, pageNumber?: number, pageSize?: number}) {} 
// }

// export class SetMessagesSent implements Action {
//     readonly type = SET_MESSAGES_SENT;
//     constructor(public payload: { messages: Message[], pagination: Pagination}) {}
// }

// export class GetMessagesReceived implements Action {
//     readonly type = GET_MESSAGES_RECEIVED;
//     constructor(public payload: { appUserId: number, pageNumber?: number, pageSize?: number}) {}
// }

// export class SetMessagesReceived implements Action {
//     readonly type = SET_MESSAGES_RECEIVED;

//     constructor(public payload: { messages: Message[], pagination: Pagination}) {}
// }

export class SetBaseInformation implements Action {
    readonly type = SET_BASE_INFORMATION;

    constructor(public payload: {message: Message, messageReplyingTo:string}){};
}

export class SendMessage implements Action {
    readonly type = SEND_MESSAGE;

    constructor(public payload: Message) {}
}

export class ClearMessage implements Action {
    readonly type = CLEAR_MESSAGE;

    constructor() {}
}

export class SetSelectedMessage implements Action {
    readonly type = SET_SELECTED_MESSAGE;

    constructor(public payload:Message){}
}

export class SetIsReadFlag implements Action {
    readonly type = SET_ISREAD_FLAG;

    constructor(public payload:number){} //messageId
}

export type MessageActions = GetMessages 
                           | SetMessages 
                        //    | GetMessagesReceived
                        //    | SetMessagesReceived 
                        //    | GetMessagesSent 
                        //    | SetMessagesSent 
                           | SetBaseInformation
                           | SendMessage 
                           | ClearMessage
                           | SetSelectedMessage
                           | SetIsReadFlag;