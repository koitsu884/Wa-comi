import { NgModule } from "@angular/core";
import { CircleHomeComponent } from "./circle-home/circle-home.component";
import { CircleEditComponent } from "./circle-edit/circle-edit.component";
import { CircleDetailsComponent } from "./circle-details/circle-details.component";
import { CircleRoutingModule } from "./circle-routing.module";
import { SharedModule } from "../shared/shared.module";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { StoreModule } from "@ngrx/store";
import { circleReducer } from "./store/circle.reducers";
import { EffectsModule } from "@ngrx/effects";
import { CircleEffects } from "./store/circle.effects";
import { CircleCategoryResolver } from "../_resolvers/circle-categories.resolver";
import { CircleInfoComponent } from "./circle-details/circle-info/circle-info.component";
import { CircleSideinfoComponent } from "./circle-details/circle-sideinfo/circle-sideinfo.component";

@NgModule({
    declarations: [
        CircleHomeComponent,
        CircleEditComponent,
        CircleDetailsComponent,
        CircleInfoComponent,
        CircleSideinfoComponent
    ],
    imports: [
        CircleRoutingModule,
        SharedModule,
        PaginationModule,
        StoreModule.forFeature('circle', circleReducer),
        EffectsModule.forFeature([CircleEffects])
    ],
    providers: [
        CircleCategoryResolver
    ]
})

export class CircleModule { }