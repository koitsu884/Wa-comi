import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { MessageHomeComponent } from "./message-home/message-home.component";
import { AuthGuard } from "../_guards/auth.guard";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { MessageSendformComponent } from "./message-sendform/message-sendform.component";
import { MessageDetailComponent } from "./message-detail/message-detail.component";
import { MessageStaticsComponent } from "./message-statics/message-statics.component";

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
        // children: [
        //     { path:'', component: MessageStaticsComponent},
        //     // { path:':userId/:type', component: MessageListComponent},
        //     { path:':userId/sent', component: MessageListSentComponent},
        //     { path:':userId/received', component: MessageListReceivedComponent},
        // ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(messageRoute)
    ],
    exports: [RouterModule]
})
export class MessageRoutingModule {}