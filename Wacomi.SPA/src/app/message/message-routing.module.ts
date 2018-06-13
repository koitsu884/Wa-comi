import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { MessageHomeComponent } from "./message-home/message-home.component";
import { AuthGuard } from "../_guards/auth.guard";
import { MessageSentlistComponent } from "./message-home/message-sentlist/message-sentlist.component";
import { MessageReceivedlistComponent } from "./message-home/message-receivedlist/message-receivedlist.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { MessageListComponent } from "./message-home/message-list/message-list.component";
import { MessageSendformComponent } from "./message-sendform/message-sendform.component";
import { MessageDetailComponent } from "./message-detail/message-detail.component";


const messageRoute: Routes = [
    {
        path: 'send',
        component:MessageSendformComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'details/:type',
        component:MessageDetailComponent
    },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:MessageHomeComponent,
        canActivate: [AuthGuard],
        resolve: {
            appUser:AppUserResolver,            
        },
        children: [
            { path:'', component: MessageListComponent},
            { path:':userId/sent', component: MessageSentlistComponent},
            { path:':userId/received', component: MessageReceivedlistComponent},
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(messageRoute)
    ],
    exports: [RouterModule]
})
export class MessageRoutingModule {}