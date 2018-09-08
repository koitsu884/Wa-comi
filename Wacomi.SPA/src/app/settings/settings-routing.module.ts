import { NgModule } from "@angular/core";
import { AuthGuard } from "../_guards/auth.guard";
import { Routes, RouterModule } from "@angular/router";
import { SettingsHomeComponent } from "./settings-home/settings-home.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../_resolvers/hometownlist.resolver";
import { MemberEditResolver } from "../_resolvers/member-edit.resolver";
import { BusinessEditResolver } from "../_resolvers/business-edit.resolver";
import { AccountEditResolver } from "../_resolvers/account-edit.resolver";


const settingRoute: Routes = [
    {
        path: ':userId',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component:SettingsHomeComponent,
        resolve: {
            account:AccountEditResolver,
            bisUser:BusinessEditResolver,
            member:MemberEditResolver,
            cities:CityListResolver,
            hometowns:HomeTownListResolver,
        },
       
    },
];

@NgModule({
    imports: [
        RouterModule.forChild(settingRoute)
    ],
    exports: [RouterModule]
})
export class SettingsRoutingModule {}