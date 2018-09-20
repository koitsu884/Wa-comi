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
        path: 'detail/:id', 
        component: CircleDetailsComponent, 
        resolve: {
            appUser:AppUserResolver
        },
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