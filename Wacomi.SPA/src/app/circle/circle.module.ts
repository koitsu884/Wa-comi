import { NgModule } from "@angular/core";
import { CircleHomeComponent } from "./circle-home/circle-home.component";
import { CircleEditComponent } from "./circle-edit/circle-edit.component";
import { CircleDetailsComponent } from "./circle-details/circle-details.component";
import { CircleRoutingModule } from "./circle-routing.module";
import { SharedModule } from "../shared/shared.module";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { StoreModule } from "@ngrx/store";
import { circleReducer, reducers } from "./store/circle.reducers";
import { EffectsModule } from "@ngrx/effects";
import { CircleEffects } from "./store/circle.effects";
import { CircleCategoryResolver } from "../_resolvers/circle-categories.resolver";
import { CircleInfoComponent } from "./circle-details/circle-info/circle-info.component";
import { CircleSideinfoComponent } from "./circle-details/circle-sideinfo/circle-sideinfo.component";
import { CircleMemberListComponent } from "./circle-member/circle-member-list/circle-member-list.component";
import { CircleMemberEffects } from "./store/circlemember.effects";
import { circleMemberReducer } from "./store/circlemember.reducers";
import { CircleRequestListComponent } from "./circle-details/circle-request-list/circle-request-list.component";
import { CircleOverviewComponent } from "./circle-details/circle-overview/circle-overview.component";
import { TabsModule } from "ngx-bootstrap";

@NgModule({
    declarations: [
        CircleHomeComponent,
        CircleEditComponent,
        CircleDetailsComponent,
        CircleInfoComponent,
        CircleSideinfoComponent,
        CircleMemberListComponent,
        CircleRequestListComponent,
        CircleOverviewComponent
    ],
    imports: [
        CircleRoutingModule,
        SharedModule,
        PaginationModule,
        TabsModule,
        StoreModule.forFeature('circleModule', reducers),
        EffectsModule.forFeature([CircleEffects, CircleMemberEffects])
    ],
    providers: [
        CircleCategoryResolver
    ]
})

export class CircleModule { }