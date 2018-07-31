import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PasswordEditComponent } from './password-edit/password-edit.component';
import { AccountRoutingModule } from './account-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AccountEffects } from './store/account.effects';
import { accountReducer } from './store/account.reducers';
import { ConfirmComponent } from './confirm/confirm.component';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { PasswordForgotComponent } from './password-forgot/password-forgot.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    PasswordEditComponent,
    PasswordForgotComponent,
    PasswordResetComponent,
    ConfirmComponent,
  ],
  imports:[
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    AccountRoutingModule,
  ]
})
export class AccountModule {}
