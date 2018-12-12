import { Component, OnInit, Input } from '@angular/core';
import { Message } from '../../../_models/Message';
import { MessageService } from '../../../_services/message.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-message-list-item',
  templateUrl: './message-list-item.component.html',
  styleUrls: ['./message-list-item.component.css']
})
export class MessageListItemComponent implements OnInit {
  @Input() message: Message;
  @Input() isSent: boolean;
  imageUrl: string;
  displayName: string;

  constructor(private messageService: MessageService, private router: Router) { }

  ngOnInit() {
    if(this.isSent){
      this.imageUrl = this.message.recipientPhotoUrl;
      this.displayName = this.message.recipientDisplayName;
    }
    else{
      this.imageUrl = this.message.senderPhotoUrl;
      this.displayName = this.message.senderDisplayName;
    }
  }

  onSelect(){
    this.messageService.setSelectedMessage(this.message);
    this.router.navigate(['/message/details/' , this.isSent ? 'sent' : 'received']);
  }
}
