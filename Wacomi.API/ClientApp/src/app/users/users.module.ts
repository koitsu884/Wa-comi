import { NgModule } from "@angular/core";
import { UserDetailComponent } from "./user-detail/user-detail.component";
import { UserCardComponent } from "./user-detail/user-card/user-card.component";
import { SharedModule } from "../shared/shared.module";
import { UsersRoutingModule } from "./users-routing.module";
import { UserDetailResolver } from "../_resolvers/user-detail.resolver";
import { UserPostsComponent } from "./user-posts/user-posts.component";
import { UserAttractionListComponent } from "./user-posts/user-attraction-list/user-attraction-list.component";
import { UserAttractionReviewListComponent } from "./user-posts/user-attraction-review-list/user-attraction-review-list.component";

@NgModule({
    declarations: [
        UserDetailComponent,
        UserCardComponent,
        UserPostsComponent,
        UserAttractionListComponent,
        UserAttractionReviewListComponent
    ],
    imports: [
        SharedModule,
        UsersRoutingModule,
    ],
    providers: [
        UserDetailResolver
    ]
})

export class UsersModule { }