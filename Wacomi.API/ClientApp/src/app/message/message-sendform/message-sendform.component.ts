
import {take} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from '../../_models/Message';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { Observable } from 'rxjs';
import * as fromMessage from '../store/message.reducers';
import * as MessageActions from '../store/message.actions';
import * as fromApp from '../../store/app.reducer';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-message-sendform',
  templateUrl: './message-sendform.component.html',
  styleUrls: ['./message-sendform.component.css']
})
export class MessageSendformComponent implements OnInit {
  message: Message;
  messageReplyingTo: string;
  // messageState: Observable<fromMessage.State>;


  constructor(private store: Store<fromApp.AppState>, private route: ActivatedRoute, private router: Router, private alertify: AlertifyService) { }

  ngOnInit() {
    this.store.select("messages").pipe(
      take(1))
      .subscribe((result) => {
        this.message = result.sendingMessage;
        this.messageReplyingTo = result.messageReplyingTo;
      })
  }

  onSubmit() {
    this.store.dispatch(new MessageActions.SendMessage(this.message));
  }

}
