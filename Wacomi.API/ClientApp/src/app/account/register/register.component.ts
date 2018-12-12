import { OnInit, Component, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from '../../_services/alertify.service';
import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { RegisterInfo } from '../../_models/RegisterInfo';
import { PasswordEditorBase } from '../passwordEditorBase';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent extends PasswordEditorBase implements OnInit {
  // passwordMinLength = 6;
  // passwordMaxLength = 20;
  registerForm: FormGroup;
  registerInfo: RegisterInfo;
  sending: boolean;
  // passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,20}";

  constructor(
              private store:Store<fromApp.AppState>,
              private alertify: AlertifyService,
              private fb: FormBuilder) { super();}
  ngOnInit(){
    this.createRegistarForm();
  }

  createRegistarForm() {
    this.registerForm = this.fb.group({
      userName:[ '', [Validators.required, Validators.pattern("^[0-9A-Za-z]{4,20}")]],
      email:['', [Validators.required, Validators.email]],
      displayName:[''],
      userType : ['Member'],
      password: ['',
                   [
                     Validators.required,
                     Validators.pattern(this.passwordPattern)]
                ],
      confirmPassword: ['', Validators.required]         
    }, {validator: this.passwordMatchValidator})
  }

  // passwordMatchValidator(g: FormGroup){
  //   return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch' : true};
  // }

  register() {
    if(this.registerForm.valid){
      this.sending = true;
      this.registerForm.markAsPristine();
      this.registerInfo = Object.assign({}, this.registerForm.value);
      this.store.dispatch(new AccountActions.TrySignup({registerInfo: this.registerInfo}));
    }
  }
}
