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
import { CoreModule } from "../core/core.module";
import { BlogModule } from "../blog/blog.module";
import { PhotoModule } from "../photo/photo.module";
import { UserPhotoResolver } from "../_resolvers/userphoto.resolver";
import { AccountEditResolver } from "../_resolvers/account-edit.resolver";
import { AccountEditComponent } from "./settings-home/account-edit/account-edit.component";
import { UserBlogResolver } from "../_resolvers/userblog.resolver";

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
        BlogModule,
        PhotoModule,
        TabsModule,
        ReactiveFormsModule,
        SettingsRoutingModule,
        BsDatepickerModule.forRoot(),
    ],
    providers:[
        BusinessEditResolver,
        MemberEditResolver,
        CityListResolver,
        HomeTownListResolver,
        UserPhotoResolver,
        UserBlogResolver,
        AccountEditResolver
    ]
})

export class SettingsModule {}