import { NgModule } from "@angular/core";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserCardComponent } from "./user-detail/user-card/user-card.component";
import { SharedModule } from "../shared/shared.module";
import { UsersRoutingModule } from "./users-routing.module";
import { UserDetailResolver } from "../_resolvers/user-detail.resolver";

@NgModule({
    declarations: [
        UserDetailComponent,
        UserCardComponent,
],
    imports: [
        SharedModule,
        UsersRoutingModule,
    ],
    providers: [
        UserDetailResolver
    ]
})

export class UsersModule {}