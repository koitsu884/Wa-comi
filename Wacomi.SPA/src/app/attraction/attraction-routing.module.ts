import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AttractionHomeComponent } from "./attraction-home/attraction-home.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { AttractionDetailsComponent } from "./attraction-details/attraction-details.component";
import { AttractionEditComponent } from "./attraction-edit/attraction-edit.component";
import { AuthGuard } from "../_guards/auth.guard";
import { MemberGuard } from "../_guards/member.guard";
import { AttractionCategoryResolver } from "../_resolvers/attraction-categories.resolver";
import { AttractionReviewEditComponent } from "./attraction-review-edit/attraction-review-edit.component";
import { AttractionReviewListComponent } from "./attraction-review-list/attraction-review-list.component";
import { AttractionResolver } from "./_resolver/attraction.resolver";

const attractionRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:AttractionHomeComponent,
        resolve: {
            appUser:AppUserResolver,            
            cities:CityListResolver,
            categories: AttractionCategoryResolver
        },
    },
    {
        path: 'detail/:id', 
        component: AttractionDetailsComponent, 
        resolve: {
            appUser:AppUserResolver
        },
    },
    {
        path: 'reviews', 
        runGuardsAndResolvers: 'always',
        component: AttractionReviewListComponent, 
        resolve: {
            attraction: AttractionResolver,
            appUser:AppUserResolver
        },
    },
    {
        path: 'edit', 
        component: AttractionEditComponent, 
        canActivate: [MemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: AttractionCategoryResolver
        },
    },
    {
        path: 'edit/:id', 
        component: AttractionEditComponent, 
        canActivate: [MemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: AttractionCategoryResolver
        },
    },
    {
        path: 'edit/:id/review', 
        component: AttractionReviewEditComponent, 
        canActivate: [AuthGuard],
        resolve: {
            appUser:AppUserResolver
        },
    },
    {
        path: 'edit/:id/review/:reviewId', 
        component: AttractionReviewEditComponent, 
        canActivate: [AuthGuard],
        resolve: {
            appUser:AppUserResolver
        },
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(attractionRoute)
    ],
    exports: [RouterModule],
    providers: [
        AttractionResolver
    ]
})
export class AttractionRoutingModule {}