import * as NotificationActions from './notification.action';
import { AppNotification } from '../../_models/Notification';

export interface State {
    notifications: AppNotification[];
}

const initialState: State = {
    notifications: []
};

export function notificationReducer(state = initialState, action: NotificationActions.NotificationActions ){
    switch(action.type){
        case NotificationActions.SET_NOTIFICATIONS:
            return {
                ...state,
                notifications: action.payload
            }
        case NotificationActions.DELETE_NOTIFICATION:
            var temp = [...state.notifications];
            var index = temp.findIndex(x => x.id == action.payload);
            temp.splice(index, 1);
            return {
                ...state,
                notifications: temp
            }
        default:
            return state;
    }
}