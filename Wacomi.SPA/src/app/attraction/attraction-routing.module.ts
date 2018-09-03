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
        path: 'edit', 
        runGuardsAndResolvers: 'always',
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
        runGuardsAndResolvers: 'always',
        component: AttractionEditComponent, 
        canActivate: [MemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: AttractionCategoryResolver
        },
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(attractionRoute)
    ],
    exports: [RouterModule]
})
export class AttractionRoutingModule {}