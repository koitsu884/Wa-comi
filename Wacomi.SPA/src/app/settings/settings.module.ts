import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { SettingsHomeComponent } from "./settings-home/settings-home.component";
import { TabsModule, BsDatepickerModule } from "ngx-bootstrap";
import { UserEditComponent } from "./settings-home/user-edit/user-edit.component";
import { FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { MemberProfileEditComponent } from "./settings-home/member-profile-edit/member-profile-edit.component";
import { BusinessProfileEditComponent } from "./settings-home/business-profile-edit/business-profile-edit.component";
import { SharedModule } from "../shared/shared.module";
import { SettingsRoutingModule } from "./settings-routing.module";
import { BusinessEditResolver } from "../_resolvers/business-edit.resolver";
import { MemberEditResolver } from "../_resolvers/member-edit.resolver";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../_resolvers/hometownlist.resolver";
import { AppUserEditResolver } from "../_resolvers/appuser-edit.resolver";
import { CoreModule } from "../core/core.module";

@NgModule({
    declarations: [
        SettingsHomeComponent,
        UserEditComponent,
        MemberProfileEditComponent,
        BusinessProfileEditComponent
    ],
    imports:[
        SharedModule,
        TabsModule,
        ReactiveFormsModule,
        SettingsRoutingModule,
        BsDatepickerModule.forRoot(),
    ],
    providers:[
        AppUserEditResolver,
        BusinessEditResolver,
        MemberEditResolver,
        CityListResolver,
        HomeTownListResolver
    ]
})

export class SettingsModule {}