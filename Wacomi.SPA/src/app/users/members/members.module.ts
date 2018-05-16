import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

import { MemberDetailComponent } from "./member-detail/member-detail.component";
import { MemberHomeComponent } from "./member-home/member-home.component";
import { MemberListComponent } from "./member-list/member-list/member-list.component";
import { TimeAgoPipe } from "time-ago-pipe";
import { TabsModule, BsDatepickerModule } from "ngx-bootstrap";
import { MembersRoutingModule } from "./members-routing.module";
import { MemberGuard } from "../../_guards/member.guard";
import { MemberEditResolver } from "../../_resolvers/member-edit.resolver";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
    declarations: [
        MemberDetailComponent,
        MemberHomeComponent,
        MemberListComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        MembersRoutingModule,
        TabsModule,
    ],
    providers: [
        MemberEditResolver,
        MemberGuard
    ]
})

export class MembersModule {}