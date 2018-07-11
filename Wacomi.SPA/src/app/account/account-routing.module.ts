import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";
import { CityListResolver } from "../_resolvers/citylist.resolver";
import { HomeTownListResolver } from "../_resolvers/hometownlist.resolver";
import { ConfirmComponent } from "./confirm/confirm.component";
import { PasswordResetComponent } from "./password-reset/password-reset.component";
import { PasswordForgotComponent } from "./password-forgot/password-forgot.component";

const accountRoute: Routes = [
    {path: 'login', component: LoginComponent},
    {
        path: 'register', 
        component: RegisterComponent, 
        resolve: {
            cities:CityListResolver,
            hometowns:HomeTownListResolver
        }
    },
    {path: 'password/forgot', component: PasswordForgotComponent},
    {path: 'password/reset/:id/:code', component: PasswordResetComponent},
    {path: 'confirm/:id/:code', component: ConfirmComponent}
];

@NgModule({
    imports: [
        RouterModule.forChild(accountRoute)
    ],
    exports: [RouterModule]
})
export class AccountRoutingModule {}