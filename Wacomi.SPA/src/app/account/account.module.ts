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


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    PasswordEditComponent,
  ],
  imports:[
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AccountRoutingModule,
  ]
})
export class AccountModule {}
