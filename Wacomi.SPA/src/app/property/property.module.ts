import { NgModule } from "@angular/core";
import { AgmCoreModule } from "@agm/core";
import { PropertyHomeComponent } from "./property-home/property-home.component";
import { PropertyEditComponent } from "./property-edit/property-edit.component";
import { PropertyDetailsComponent } from "./property-details/property-details.component";
import { PropertyRoutingModule } from "./property-routing.module";
import { SharedModule } from "../shared/shared.module";
import { PaginationModule } from "ngx-bootstrap/pagination";
import { StoreModule } from "@ngrx/store";
import { propertyReducer } from "./store/property.reducers";
import { EffectsModule } from "@ngrx/effects";
import { PropertyEffects } from "./store/property.effects";
import { PropertyCategoryResolver } from "../_resolvers/property-categories.resolver";
import { PropertyDetailInfoComponent } from "./property-details/property-detail-info/property-detail-info.component";
import { PropertyOptionsComponent } from "./property-details/property-options/property-options.component";

@NgModule({
    declarations: [
        PropertyHomeComponent,
        PropertyEditComponent,
        PropertyDetailsComponent,
        PropertyDetailInfoComponent,
        PropertyOptionsComponent
    ],
    imports: [
        PropertyRoutingModule,
        SharedModule,
        PaginationModule,
        AgmCoreModule,
        StoreModule.forFeature('property', propertyReducer),
        EffectsModule.forFeature([PropertyEffects])
    ],
    providers: [
        PropertyCategoryResolver
    ]
})

export class PropertyModule { }