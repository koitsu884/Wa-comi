import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BusinessHomeComponent } from "./business-home/business-home.component";
import { BusinessDetailComponent } from "./business-detail/business-detail.component";
import { TabsModule } from "ngx-bootstrap";
import { FormsModule } from "@angular/forms";
import { BusinessesRoutingModule } from "./businesses-routing.module";
import { BusinessGuard } from "../../_guards/business.guard";
import { BusinessEditResolver } from "../../_resolvers/business-edit.resolver";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
    declarations: [
        BusinessHomeComponent,
        BusinessDetailComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        BusinessesRoutingModule,
        TabsModule,
    ],
    providers: [
        BusinessEditResolver,
        BusinessGuard
    ]
})

export class BusinessesModule {}