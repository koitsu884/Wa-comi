import { NgModule } from "@angular/core";
import { ClanHomeComponent } from "./clan-home/clan-home.component";
import { SharedModule } from "../shared/shared.module";
import { ClanDetailComponent } from './clan-detail/clan-detail.component';
import { ClanEditComponent } from "./clan-edit/clan-edit.component";
import { ClanSeekCategoryResolver } from "../_resolvers/clanseek-categories.resolver";
import { ClanListComponent } from "./clan-home/clan-list/clan-list.component";
import { ClanRoutingModule } from "./clan-routing.module";
import { StoreModule } from "@ngrx/store";
import { EffectsModule } from "@ngrx/effects";
import { clanSeekReducer } from "./store/clan.reducers";
import { ClanSeekEffects } from "./store/clan.effects";
import { ClanDetailCardComponent } from "./clan-detail/clan-detail-card/clan-detail-card.component";
import { ClanDetailInfoComponent } from "./clan-detail/clan-detail-info/clan-detail-info.component";
import { ClanSeekEditResolver } from "../_resolvers/clanseek-edit.resolver";
import { ClanSeekResolver } from "./_resolver/clanseek.resolver";
import { PaginationModule } from "ngx-bootstrap";

@NgModule({
    declarations: [
        ClanHomeComponent,
        ClanDetailComponent,
        ClanEditComponent,
        ClanDetailCardComponent,
        ClanDetailInfoComponent,
        ClanListComponent
    ],
    imports: [
        ClanRoutingModule,
        SharedModule,
        PaginationModule,
        StoreModule.forFeature('clan', clanSeekReducer),
        EffectsModule.forFeature([ClanSeekEffects])
    ],
    providers: [
        ClanSeekCategoryResolver,
        ClanSeekResolver,
        ClanSeekEditResolver
    ]
})

export class ClanModule { }