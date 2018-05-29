import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { MemberGuard } from "../../_guards/member.guard";
import { MemberHomeComponent } from "./member-home/member-home.component";
import { MemberEditResolver } from "../../_resolvers/member-edit.resolver";
import { CityListResolver } from "../../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../../_resolvers/hometownlist.resolver";
import { MemberListComponent } from "./member-list/member-list/member-list.component";
import { MemberDetailComponent } from "./member-detail/member-detail.component";
import { MemberDetailResolver } from "../../_resolvers/member-detail.resolver";

const membersRoutes: Routes = [
    // {
    //     path: '',
    //     runGuardsAndResolvers: 'always',
    //     canActivate: [MemberGuard],
    //     children: [
    //         {path: 'home', component: MemberHomeComponent, resolve: {member:MemberEditResolver}},
    //    ]
    // },
    {
        path:'details/:id',
        runGuardsAndResolvers: 'always',
        component: MemberDetailComponent,
        resolve: {member:MemberDetailResolver}
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(membersRoutes)
    ],
    exports: [RouterModule]
})
export class MembersRoutingModule {}