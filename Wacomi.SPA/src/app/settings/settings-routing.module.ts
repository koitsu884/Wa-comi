import { NgModule } from "@angular/core";
import { AuthGuard } from "../_guards/auth.guard";
import { Routes, RouterModule } from "@angular/router";
import { SettingsHomeComponent } from "./settings-home/settings-home.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../_resolvers/hometownlist.resolver";
import { MemberEditResolver } from "../_resolvers/member-edit.resolver";
import { BusinessEditResolver } from "../_resolvers/business-edit.resolver";
import { AppUserEditResolver } from "../_resolvers/appuser-edit.resolver";
import { MemberGuard } from "../_guards/member.guard";
import { MemberProfileEditComponent } from "./settings-home/member-profile-edit/member-profile-edit.component";
import { BusinessGuard } from "../_guards/business.guard";
import { BusinessProfileEditComponent } from "./settings-home/business-profile-edit/business-profile-edit.component";
import { UserEditComponent } from "./settings-home/user-edit/user-edit.component";


const settingRoute: Routes = [
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        component:SettingsHomeComponent,
        resolve: {
            appUser:AppUserEditResolver,
            bisUser:BusinessEditResolver,
            member:MemberEditResolver,
            cities:CityListResolver,
            hometowns:HomeTownListResolver
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