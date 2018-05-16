import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { BusinessGuard } from "../../_guards/business.guard";
import { BusinessHomeComponent } from "./business-home/business-home.component";
import { BusinessEditResolver } from "../../_resolvers/business-edit.resolver";
import { CityListResolver } from "../../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../../_resolvers/hometownlist.resolver";

const membersRoutes: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [BusinessGuard],
        children: [
            {path: 'home', component: BusinessHomeComponent, resolve: {bisuser:BusinessEditResolver}},
        ]
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(membersRoutes)
    ],
    exports: [RouterModule]
})
export class BusinessesRoutingModule {}