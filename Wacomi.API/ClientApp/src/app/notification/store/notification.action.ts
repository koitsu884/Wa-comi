import { Action } from "@ngrx/store";
import { AppNotification } from "../../_models/Notification";

export const GET_NOTIFICATIONS = 'GET_NOTIFICATIONS';
export const SET_NOTIFICATIONS = 'SET_NOTIFICATIONS';
export const TRY_DELETE_NOTIFICATION = 'TRY_DELETE_NOTIFICATION';
export const DELETE_NOTIFICATION = 'DELETE_NOTIFICATION';
export const DELETE_ALL_NOTIFICATIONS = 'DELETE_ALL_NOTIFICATIONS';

export class GetNotifications implements Action {
    readonly type = GET_NOTIFICATIONS;
    constructor(public payload: number) {}//appUserId
}

export class SetNotifications implements Action {
    readonly type = SET_NOTIFICATIONS;
    constructor(public payload: AppNotification[]) {}
}

export class TryDeleteNotification implements Action {
    readonly type = TRY_DELETE_NOTIFICATION;
    constructor(public payload: number) {}//id
}

export class DeleteNotification implements Action {
    readonly type = DELETE_NOTIFICATION;
    constructor(public payload: number) {}//id
}

export class DeleteAllNotifications implements Action {
    readonly type = DELETE_ALL_NOTIFICATIONS;
    constructor(public payload: number) {}//appUserId
}

export type NotificationActions = GetNotifications 
                          | SetNotifications
                          | TryDeleteNotification 
                          | DeleteNotification 
                          | DeleteAllNotifications;