
import {take} from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { GlobalService } from '../../_services/global.service';
import { AlertifyService } from '../../_services/alertify.service';
import { AppUser } from '../../_models/AppUser';
import { Store } from '@ngrx/store';
import * as fromApp from '../../store/app.reducer';
import { Router } from '@angular/router';
import { PasswordEditorBase } from '../passwordEditorBase';

@Component({
  selector: 'app-password-edit',
  templateUrl: './password-edit.component.html',
  styleUrls: ['./password-edit.component.css']
})
export class PasswordEditComponent extends PasswordEditorBase implements OnInit {
  userId: string;
  changePasswordForm : FormGroup;
  // passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,20}";
  // passwordMinLength = 6;
  // passwordMaxLength = 20;

  constructor(private formBuilder: FormBuilder, 
    private globalService: GlobalService, 
    private alertify: AlertifyService,
    private router: Router,
    private store : Store<fromApp.AppState>) {super();}

  ngOnInit() {
    this.store.select("account").pipe(
              take(1))
              .subscribe((accountState) => {
                this.userId = accountState.account.id;
              });
    this.createChangePasswordForm();
  }

  createChangePasswordForm() {
    this.changePasswordForm = this.formBuilder.group({
      currentPassword: ['', Validators.required],
      password: ['',
                   [
                     Validators.required,
                     Validators.pattern(this.passwordPattern)]
                ],
      confirmPassword: ['', Validators.required]         
    }, {validator: this.passwordMatchValidator})
  }

  // passwordMatchValidator(g: FormGroup){
  //   return g.get('newPassword').value === g.get('confirmPassword').value ? null : {'mismatch' : true};
  // }
  
  submit(){
    if(this.changePasswordForm.valid){
      let values = Object.assign({}, this.changePasswordForm.value);
      this.globalService.sendChangePasswordRequest(this.userId, values.currentPassword, values.password)
        .subscribe(() => {
          this.alertify.success("パスワードを変更しました");
          this.router.navigate(['/settings', this.userId]);
        }, (error) => {
          this.alertify.error(error);
        })
      // this.store.dispatch(new AccountActions.TrySignup({registerInfo: this.registerInfo}));
    }
  }
}
