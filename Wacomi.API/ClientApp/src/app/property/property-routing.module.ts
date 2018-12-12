import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { PropertyHomeComponent } from "./property-home/property-home.component";
import { AppUserResolver } from "../_resolvers/appuser.resolver";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { PropertyCategoryResolver } from "../_resolvers/property-categories.resolver";
import { PropertyEditComponent } from "./property-edit/property-edit.component";
import { MemberGuard } from "../_guards/member.guard";
import { PropertyDetailsComponent } from "./property-details/property-details.component";

const propertyRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        component:PropertyHomeComponent,
        resolve: {
            appUser:AppUserResolver,            
            cities:CityListResolver,
            categories: PropertyCategoryResolver
        },
    },
    {
        path: 'detail/:id', 
        component: PropertyDetailsComponent, 
        resolve: {
            appUser:AppUserResolver
        },
    },
    {
        path: 'edit', 
        component: PropertyEditComponent, 
        canActivate: [MemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: PropertyCategoryResolver
        },
    },
    {
        path: 'edit/:id', 
        component: PropertyEditComponent, 
        canActivate: [MemberGuard],
        resolve: {
            appUser:AppUserResolver,
            cities:CityListResolver,
            categories: PropertyCategoryResolver
        },
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(propertyRoute)
    ],
    exports: [RouterModule],
    // providers: [
    //     AttractionResolver
    // ]
})
export class PropertyRoutingModule {}