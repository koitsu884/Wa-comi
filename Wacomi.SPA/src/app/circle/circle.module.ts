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

@NgModule({
    declarations: [
        CircleHomeComponent,
        CircleEditComponent,
        CircleDetailsComponent
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