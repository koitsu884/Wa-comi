import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Store } from '@ngrx/store';
import * as fromApp from '../store/app.reducer';
import * as MessageActions from '../message/store/message.actions';
import { Message } from '../_models/Message';

@Injectable()
export class MessageService {
    baseUrl = environment.apiUrl;

    constructor(private httpClient : HttpClient, private store: Store<fromApp.AppState>){
    }

    preparSendingeMessage(message: Message, messageReplyingTo: string){
        this.store.dispatch(new MessageActions.SetBaseInformation({message:message, messageReplyingTo:messageReplyingTo}));
    }

    setSelectedMessage(message: Message){
        this.store.dispatch(new MessageActions.SetSelectedMessage(message));
    }
}
