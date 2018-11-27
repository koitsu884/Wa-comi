import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { CircleHomeComponent } from "./circle-home/circle-home.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { CircleCategoryResolver } from "../_resolvers/circle-categories.resolver";
import { CircleDetailsComponent } from "./circle-details/circle-details.component";
import { CircleEditComponent } from "./circle-edit/circle-edit.component";
import { AuthGuard } from "../_guards/auth.guard";
import { CircleMemberListComponent } from "./circle-member/circle-member-list/circle-member-list.component";
import { CircleOverviewComponent } from "./circle-details/circle-overview/circle-overview.component";
import { CircleRequestListComponent } from "./circle-details/circle-request-list/circle-request-list.component";
import { CircleTopicComponent } from "./circle-details/circle-topic/circle-topic.component";
import { CircleTopicEditComponent } from "./circle-details/circle-topic/circle-topic-edit/circle-topic-edit.component";
import { CircleTopicDetailComponent } from "./circle-details/circle-topic/circle-topic-detail/circle-topic-detail.component";
import { CircleManagementComponent } from "./circle-management/circle-management.component";
import { CircleMemberGuard } from "./_guard/circlemember.guard";
import { CircleOwnerGuard } from "./_guard/circleowner.guard";
import { CircleEventDetailComponent } from "./circle-details/circle-event/circle-event-detail/circle-event-detail.component";
import { CircleEventEditComponent } from "./circle-details/circle-event/circle-event-edit/circle-event-edit.component";
import { CircleEventComponent } from "./circle-details/circle-event/circle-event.component";
import { CircleEventParticipationsComponent } from "./circle-details/circle-event/circle-event-detail/circle-event-participations/circle-event-participations.component";
import { CircleEventResolver } from "./_resolver/circleevent.resolver";


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
            {path: 'member', component: CircleMemberListComponent, canActivate: [CircleMemberGuard]},
            {path: 'topic', component: CircleTopicComponent, canActivate: [CircleMemberGuard]}, 
            {path: 'topic/detail/:id', component: CircleTopicDetailComponent, canActivate: [CircleMemberGuard]}, 
            {path: 'topic/detail/:id/:forcusCommentId', component: CircleTopicDetailComponent, canActivate: [CircleMemberGuard]}, 
            {path: 'topic/edit', component: CircleTopicEditComponent, canActivate: [CircleMemberGuard]}, 
            {path: 'topic/edit/:id', component: CircleTopicEditComponent, canActivate: [CircleMemberGuard]}, 
            {path: 'event', component: CircleEventComponent, canActivate: [AuthGuard]}, 
            {path: 'event/detail/:id', component: CircleEventDetailComponent, canActivate: [AuthGuard]}, 
            {path: 'event/detail/:id/:forcusCommentId', component: CircleEventDetailComponent, canActivate: [AuthGuard]}, 
            {path: 'event/detail/:id/participants', component: CircleEventParticipationsComponent, canActivate: [AuthGuard], resolve: {appUser:AppUserResolver, circleEvent:CircleEventResolver}}, 
            {path: 'event/edit', component: CircleEventEditComponent, canActivate: [CircleMemberGuard], resolve: {cities:CityListResolver}}, 
            {path: 'event/edit/:id', component: CircleEventEditComponent, canActivate: [CircleMemberGuard], resolve: {cities:CityListResolver}}, 
            {path: 'request', component: CircleRequestListComponent, canActivate: [CircleOwnerGuard]},
          ]
    },
    {
        path: 'edit', 
        component: CircleEditComponent, 
        canActivate: [CircleMemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: CircleCategoryResolver
        },
    },
    {
        path: 'edit/:id', 
        component: CircleEditComponent, 
        canActivate: [CircleOwnerGuard],
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