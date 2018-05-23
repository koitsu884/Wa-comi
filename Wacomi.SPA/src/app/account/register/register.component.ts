import { OnInit, Component, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from '../../_services/alertify.service';
import * as fromApp from '../../store/app.reducer';
import * as fromAccount from '../../account/store/account.reducers';
import * as AccountActions from '../../account/store/account.actions';
import { Store } from '@ngrx/store';
import { AppUser } from '../../_models/AppUser';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  passwordMinLength = 6;
  passwordMaxLength = 20;
  registerForm: FormGroup;
  user: AppUser;
  passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,20}";

  constructor(
              private store:Store<fromApp.AppState>,
              private alertify: AlertifyService,
              private fb: FormBuilder) { }
  ngOnInit(){
    this.createRegistarForm();
  }

  createRegistarForm() {
    this.registerForm = this.fb.group({
      userName:[ '', Validators.required],
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

  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch' : true};
  }

  register() {
    if(this.registerForm.valid){
      this.user = Object.assign({}, this.registerForm.value);
      this.store.dispatch(new AccountActions.TrySignup({registerInfo: this.user}));
    }
  }
}
