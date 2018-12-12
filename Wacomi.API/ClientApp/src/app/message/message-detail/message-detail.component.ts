
import {take} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Message } from '../../_models/Message';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import * as MessageAction from '../../message/store/message.actions';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from '../../_services/message.service';


@Component({
  selector: 'app-message-detail',
  templateUrl: './message-detail.component.html',
  styleUrls: ['./message-detail.component.css']
})
export class MessageDetailComponent implements OnInit {
  message: Message;
  type: string; //sent or received

  imageUrl: string;
  displayName: string;
  userId: number;
  constructor(private store: Store<fromApp.AppState>, private router: Router, private route: ActivatedRoute, private messageService: MessageService) { }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.type = params["type"];
      this.store.select("messages").pipe(take(1))
        .subscribe((messageState) => {
          this.message = messageState.selectedMessage;
          if (this.type == 'sent') {
            this.imageUrl = this.message.recipientPhotoUrl;
            this.displayName = this.message.recipientDisplayName;
            this.userId = this.message.recipientId;
          } else {
            this.imageUrl = this.message.senderPhotoUrl;
            this.displayName = this.message.senderDisplayName;
            this.userId = this.message.senderId;
            if(!this.message.isRead){
              this.store.dispatch(new MessageAction.SetIsReadFlag(this.message.id));
            }
          }
        })
    });
  }

  onReply() {
    this.messageService.preparSendingeMessage(
      {
        title: this.message.title,
        recipientDisplayName: this.message.senderDisplayName,
        recipientId: this.message.senderId,
        senderId: this.message.recipientId
      },
      "<p class='text-info'>以下のメッセージに対して返信します</p>"
       + "<h5>送信者：" + this.message.senderDisplayName + "</h5>"
       + this.message.content
      
    );
    this.router.navigate(['/message/send']);

  }

}
