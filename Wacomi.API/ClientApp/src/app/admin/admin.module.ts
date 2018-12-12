import { NgModule } from "@angular/core";
import { AdminHomeComponent } from "./admin-home/admin-home.component";
import { SharedModule } from "../shared/shared.module";
import { AdminRoutingModule } from "./admin-routing.module";
import { AdminGuard } from "../_guards/admin.guard";

@NgModule({
    declarations: [
        AdminHomeComponent
    ],
    imports:[
        AdminRoutingModule,
        SharedModule,
    ],
    providers: [
        AdminGuard,
    ]
})

export class AdminModule {}