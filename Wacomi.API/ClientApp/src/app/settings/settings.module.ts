import { NgModule, LOCALE_ID } from "@angular/core";
import { SettingsHomeComponent } from "./settings-home/settings-home.component";
import { TabsModule, BsDatepickerModule } from "ngx-bootstrap";
import { UserEditComponent } from "./settings-home/user-edit/user-edit.component";
import { ReactiveFormsModule } from "@angular/forms";
import { MemberProfileEditComponent } from "./settings-home/member-profile-edit/member-profile-edit.component";
import { BusinessProfileEditComponent } from "./settings-home/business-profile-edit/business-profile-edit.component";
import { SharedModule } from "../shared/shared.module";
import { SettingsRoutingModule } from "./settings-routing.module";
import { BusinessEditResolver } from "../_resolvers/business-edit.resolver";
import { MemberEditResolver } from "../_resolvers/member-edit.resolver";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../_resolvers/hometownlist.resolver";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";
import { AccountEditResolver } from "../_resolvers/account-edit.resolver";
import { AccountEditComponent } from "./settings-home/account-edit/account-edit.component";
import { registerLocaleData } from "@angular/common";

@NgModule({
    declarations: [
        SettingsHomeComponent,
        AccountEditComponent,
        UserEditComponent,
        MemberProfileEditComponent,
        BusinessProfileEditComponent
    ],
    imports:[
        SharedModule,
        TabsModule,
        ReactiveFormsModule,
        SettingsRoutingModule,
        BsDatepickerModule,
    ],
    providers:[
        BusinessEditResolver,
        MemberEditResolver,
        CityListResolver,
        HomeTownListResolver,
        UserPhotoResolver,
        AccountEditResolver
    ]
})

export class SettingsModule {}