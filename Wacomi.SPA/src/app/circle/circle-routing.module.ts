import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CircleHomeComponent } from "./circle-home/circle-home.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { CircleCategoryResolver } from "../_resolvers/circle-categories.resolver";
import { CircleDetailsComponent } from "./circle-details/circle-details.component";
import { CircleEditComponent } from "./circle-edit/circle-edit.component";
import { MemberGuard } from "../_guards/member.guard";
import { AppUserGuard } from "../_guards/appuser.guard";
import { AuthGuard } from "../_guards/auth.guard";
import { CircleMemberListComponent } from "./circle-member/circle-member-list/circle-member-list.component";
import { CircleOverviewComponent } from "./circle-details/circle-overview/circle-overview.component";
import { CircleRequestListComponent } from "./circle-details/circle-request-list/circle-request-list.component";
import { CircleTopicComponent } from "./circle-details/circle-topic/circle-topic.component";
import { CircleTopicEditComponent } from "./circle-details/circle-topic/circle-topic-edit/circle-topic-edit.component";
import { CircleTopicDetailComponent } from "./circle-details/circle-topic/circle-topic-detail/circle-topic-detail.component";
import { CircleManagementComponent } from "./circle-management/circle-management.component";


const circleRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:CircleHomeComponent,
        resolve: {
            appUser:AppUserResolver,            
            cities:CityListResolver,
            categories: CircleCategoryResolver
        },
    },
    {
        path: 'management',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component:CircleManagementComponent,
        resolve: {
            appUser:AppUserResolver
        },
    },
    {
        path: 'detail/:id', 
        component: CircleDetailsComponent, 
        resolve: {
            appUser:AppUserResolver
        },
        children: [
            {path: '', redirectTo: 'overview'}, 
            {path: 'overview', component: CircleOverviewComponent}, 
            {path: 'member', component: CircleMemberListComponent, canActivate: [AuthGuard]}, //TODO: add Circle member guard
            {path: 'topic', component: CircleTopicComponent, canActivate: [AuthGuard]}, 
            {path: 'topic/detail/:id', component: CircleTopicDetailComponent, canActivate: [AuthGuard]}, 
            {path: 'topic/detail/:id/:forcusCommentId', component: CircleTopicDetailComponent, canActivate: [AuthGuard]}, 
            {path: 'topic/edit', component: CircleTopicEditComponent, canActivate: [AuthGuard]}, 
            {path: 'topic/edit/:id', component: CircleTopicEditComponent, canActivate: [AuthGuard]}, 
            {path: 'event', redirectTo: 'overview'},
            {path: 'request', component: CircleRequestListComponent, canActivate: [AuthGuard]},
          ]
    },
    {
        path: 'detail/:id/members', 
        component: CircleMemberListComponent, 
        canActivate: [AuthGuard]
    },
    {
        path: 'edit', 
        component: CircleEditComponent, 
        canActivate: [AuthGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: CircleCategoryResolver
        },
    },
    {
        path: 'edit/:id', 
        component: CircleEditComponent, 
        canActivate: [AuthGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: CircleCategoryResolver
        },
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(circleRoute)
    ],
    exports: [RouterModule],
    // providers: [
    //     AttractionResolver
    // ]
})
export class CircleRoutingModule {}