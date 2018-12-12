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
import { TabsModule, ModalModule, BsDatepickerModule } from "ngx-bootstrap";
import { CircleTopicComponent } from "./circle-details/circle-topic/circle-topic.component";
import { CircleInfoComponent } from "./circle-details/circle-overview/circle-info/circle-info.component";
import { CircleSideinfoComponent } from "./circle-details/circle-overview/circle-sideinfo/circle-sideinfo.component";
import { CircleTopicEditComponent } from "./circle-details/circle-topic/circle-topic-edit/circle-topic-edit.component";
import { CircleTopicDetailComponent } from "./circle-details/circle-topic/circle-topic-detail/circle-topic-detail.component";
import { CircleTopicEffects } from "./store/circletopic.effects";
import { CircleManagementComponent } from "./circle-management/circle-management.component";
import { CircleManagementEffects } from "./store/circle-management.effects";
import { CircleMemberGuard } from "./_guard/circlemember.guard";
import { CircleOwnerGuard } from "./_guard/circleowner.guard";
import { CircleEventComponent } from "./circle-details/circle-event/circle-event.component";
import { CircleEventCalendarComponent } from "./circle-details/circle-event/circle-event-calendar/circle-event-calendar.component";
import { CircleEventEditComponent } from "./circle-details/circle-event/circle-event-edit/circle-event-edit.component";
import { CircleEventDetailComponent } from "./circle-details/circle-event/circle-event-detail/circle-event-detail.component";
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { CircleEventEffects } from "./store/circleevent.effects";
import { CircleEventParticipationsComponent } from "./circle-details/circle-event/circle-event-detail/circle-event-participations/circle-event-participations.component";
import { CircleEventParticipationEffects } from "./store/circle-event-participations.effects";
import { CircleEventResolver } from "./_resolver/circleevent.resolver";
import { CircleSearchComponent } from "./circle-home/circle-search/circle-search.component";
import { CircleEventSearchComponent } from "./circle-home/circle-event-search/circle-event-search.component";
import { CircleEventParticipationsListComponent } from "./circle-details/circle-event/circle-event-detail/circle-event-participations/circle-event-participations-list/circle-event-participations-list.component";

@NgModule({
    declarations: [
        CircleHomeComponent,
        CircleSearchComponent,
        CircleEventSearchComponent,
        CircleEditComponent,
        CircleDetailsComponent,
        CircleEventComponent,
        CircleEventCalendarComponent,
        CircleEventEditComponent,
        CircleEventDetailComponent,
        CircleInfoComponent,
        CircleSideinfoComponent,
        CircleMemberListComponent,
        CircleTopicComponent,
        CircleTopicEditComponent,
        CircleTopicDetailComponent,
        CircleRequestListComponent,
        CircleOverviewComponent,
        CircleManagementComponent,
        CircleEventParticipationsComponent,
        CircleEventParticipationsListComponent
    ],
    imports: [
        CircleRoutingModule,
        SharedModule,
        PaginationModule,
        TabsModule,
        ModalModule,
        BsDatepickerModule,
        InfiniteScrollModule,
        StoreModule.forFeature('circleModule', reducers),
        EffectsModule.forFeature([CircleEffects, CircleMemberEffects, CircleTopicEffects, CircleManagementEffects, CircleEventEffects, CircleEventParticipationEffects])
    ],
    providers: [
        CircleCategoryResolver,
        CircleEventResolver,
        CircleMemberGuard,
        CircleOwnerGuard
    ]
})

export class CircleModule { }