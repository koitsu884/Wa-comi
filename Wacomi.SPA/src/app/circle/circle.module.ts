import { NgModule } from "@angular/core";
import { CircleHomeComponent } from "./circle-home/circle-home.component";
import { CircleEditComponent } from "./circle-edit/circle-edit.component";
import { CircleDetailsComponent } from "./circle-details/circle-details.component";
import { CircleRoutingModule } from "./circle-routing.module";
import { SharedModule } from "../shared/shared.module";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { StoreModule } from "@ngrx/store";
import { reducers } from "./store/circle.reducers";
import { EffectsModule } from "@ngrx/effects";
import { CircleEffects } from "./store/circle.effects";
import { CircleCategoryResolver } from "../_resolvers/circle-categories.resolver";
import { CircleMemberListComponent } from "./circle-member/circle-member-list/circle-member-list.component";
import { CircleMemberEffects } from "./store/circlemember.effects";
import { CircleRequestListComponent } from "./circle-details/circle-request-list/circle-request-list.component";
import { CircleOverviewComponent } from "./circle-details/circle-overview/circle-overview.component";
import { TabsModule, ModalModule } from "ngx-bootstrap";
import { CircleTopicComponent } from "./circle-details/circle-topic/circle-topic.component";
import { CircleInfoComponent } from "./circle-details/circle-overview/circle-info/circle-info.component";
import { CircleSideinfoComponent } from "./circle-details/circle-overview/circle-sideinfo/circle-sideinfo.component";
import { CircleTopicEditComponent } from "./circle-details/circle-topic/circle-topic-edit/circle-topic-edit.component";
import { CircleTopicDetailComponent } from "./circle-details/circle-topic/circle-topic-detail/circle-topic-detail.component";
import { CircleTopicEffects } from "./store/circletopic.effects";
import { CircleTopicCommentComponent } from "./circle-details/circle-topic/circle-topic-detail/circle-topic-comment/circle-topic-comment.component";

@NgModule({
    declarations: [
        CircleHomeComponent,
        CircleEditComponent,
        CircleDetailsComponent,
        CircleInfoComponent,
        CircleSideinfoComponent,
        CircleMemberListComponent,
        CircleTopicComponent,
        CircleTopicEditComponent,
        CircleTopicDetailComponent,
        CircleTopicCommentComponent,
        CircleRequestListComponent,
        CircleOverviewComponent
    ],
    imports: [
        CircleRoutingModule,
        SharedModule,
        PaginationModule,
        TabsModule,
        ModalModule,
        StoreModule.forFeature('circleModule', reducers),
        EffectsModule.forFeature([CircleEffects, CircleMemberEffects, CircleTopicEffects])
    ],
    providers: [
        CircleCategoryResolver
    ]
})

export class CircleModule { }