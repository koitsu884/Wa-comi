import { NgModule } from "@angular/core";
import { MessageHomeComponent } from "./message-home/message-home.component";
import { SharedModule } from "../shared/shared.module";
import { MessageRoutingModule } from "./message-routing.module";
import { MessageSendformComponent } from './message-sendform/message-sendform.component';
import { MessageListItemComponent } from "./message-home/message-list-item/message-list-item.component";
import { MessageDetailComponent } from './message-detail/message-detail.component';
import { PaginationModule } from "ngx-bootstrap";
import { MessageStaticsComponent } from './message-statics/message-statics.component';

@NgModule({
    declarations: [
        MessageHomeComponent,
        MessageSendformComponent,
        MessageListItemComponent,
        MessageDetailComponent,
        MessageStaticsComponent,
    ],
    imports: [
        MessageRoutingModule,
        SharedModule,
        PaginationModule,
    ],
    providers: [
    ]
})

export class MessageModule { }