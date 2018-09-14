import { ActionReducerMap } from '@ngrx/store';
import * as fromAccount from '../account/store/account.reducers';
import * as fromNotification from '../notification/store/notification.reducer';
import * as fromMessage from '../message/store/message.reducers';
import * as fromGlobal from './global.reducers';

export interface AppState {
  global: fromGlobal.State;
  notification: fromNotification.State
  account: fromAccount.State;
  messages: fromMessage.State;
}

export const reducers: ActionReducerMap<AppState> = {
  global: fromGlobal.globalReducer,
  notification: fromNotification.notificationReducer,
  account: fromAccount.accountReducer,
  messages: fromMessage.messageReducer,
};
