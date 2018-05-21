import { NgModule } from "@angular/core";
import { ClanHomeComponent } from "./clan-home/clan-home.component";
import { SharedModule } from "../shared/shared.module";
import { ClanDetailComponent } from './clan-detail/clan-detail.component';
import { ClanEditComponent } from "./clan-edit/clan-edit.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { ClanSeekCategoryResolver } from "../_resolvers/clanseek-categories.resolver";
import { ClanListComponent } from "./clan-home/clan-list/clan-list.component";
import { ClanRoutingModule } from "./clan-routing.module";

@NgModule({
    declarations: [
        ClanHomeComponent,
        ClanDetailComponent,
        ClanEditComponent,
        ClanListComponent,
    ],
    imports: [
        ClanRoutingModule,
        SharedModule,
    ],
    providers: [

        // AppUserEditResolver,
        // BusinessEditResolver,
        // MemberEditResolver,
        CityListResolver,
        ClanSeekCategoryResolver,
    ]
})

export class ClanModule { }