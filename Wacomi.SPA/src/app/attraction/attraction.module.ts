import { NgModule } from "@angular/core";
import { AttractionHomeComponent } from "./attraction-home/attraction-home.component";
import { AttractionDetailsComponent } from "./attraction-details/attraction-details.component";
import { AttractionEditComponent } from "./attraction-edit/attraction-edit.component";
import { AttractionRoutingModule } from "./attraction-routing.module";
import { SharedModule } from "../shared/shared.module";
import { PaginationModule } from "ngx-bootstrap";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { attractionReducer } from "./store/attraction.reducers";
import { AttractionEffects } from "./store/attraction.effects";
import { AttractionCategoryResolver } from "../_resolvers/attraction-categories.resolver";
import { AttractionListComponent } from "./attraction-home/attraction-list/attraction-list.component";
import { AttractionDetailsCardComponent } from "./attraction-details/attraction-details-card/attraction-details-card.component";
import { AttractionDetailsInfoComponent } from "./attraction-details/attraction-details-info/attraction-details-info.component";
import { AttractionReviewCardComponent } from "./attraction-details/attraction-review-card/attraction-review-card.component";
import { AttractionReviewEditComponent } from "./attraction-review-edit/attraction-review-edit.component";
import { AttractionReviewListComponent } from "./attraction-review-list/attraction-review-list.component";
import { AttractionReviewComponent } from "./attraction-review-list/attraction-review/attraction-review.component";

@NgModule({
    declarations: [
        AttractionHomeComponent,
        AttractionDetailsComponent,
        AttractionEditComponent,
        AttractionListComponent,
        AttractionDetailsCardComponent,
        AttractionDetailsInfoComponent,
        AttractionReviewCardComponent,
        AttractionReviewEditComponent,
        AttractionReviewListComponent,
        AttractionReviewComponent
    ],
    imports: [
        AttractionRoutingModule,
        SharedModule,
        PaginationModule,
        StoreModule.forFeature('attraction', attractionReducer),
        EffectsModule.forFeature([AttractionEffects])
    ],
    providers: [
        AttractionCategoryResolver
    ]
})

export class AttractionModule { }