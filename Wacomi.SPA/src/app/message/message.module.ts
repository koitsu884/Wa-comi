import { NgModule } from "@angular/core";
import { MessageHomeComponent } from "./message-home/message-home.component";
import { SharedModule } from "../shared/shared.module";
import { MessageRoutingModule } from "./message-routing.module";
import { MessageSentlistComponent } from "./message-home/message-sentlist/message-sentlist.component";
import { MessageReceivedlistComponent } from "./message-home/message-receivedlist/message-receivedlist.component";
import { MessageListComponent } from "./message-home/message-list/message-list.component";
import { MessageSendformComponent } from './message-sendform/message-sendform.component';
import { MessageListItemComponent } from "./message-home/message-list-item/message-list-item.component";
import { MessageDetailComponent } from './message-detail/message-detail.component';

@NgModule({
    declarations: [
        MessageHomeComponent,
        MessageSentlistComponent,
        MessageReceivedlistComponent,
        MessageListComponent,
        MessageSendformComponent,
        MessageListItemComponent,
    MessageDetailComponent
],
    imports: [
        MessageRoutingModule,
        SharedModule,
    ],
    providers: [
    ]
})

export class MessageModule { }