import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalService } from '../../_services/global.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from '../../_services/alertify.service';
@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent implements OnInit {
  newPasswordForm: FormGroup;
  passwordMinLength = 6;
  passwordMaxLength = 20;

  code: string;
  userId: string;
  password:string;
  passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,20}";

  constructor(private route: ActivatedRoute, 
    private globalService: GlobalService, 
    private alertify: AlertifyService,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.createNewPasswordForm();
    this.route.params.subscribe((params) => {
      this.code = params["code"];
      this.userId = params["id"];
    })
  }

  createNewPasswordForm() {
    this.newPasswordForm = this.fb.group({
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

  submit(){
    if(this.newPasswordForm.valid){
      let values = Object.assign({}, this.newPasswordForm.value);
      console.log(this.userId);
      console.log(this.code);
      console.log(values);
      this.globalService.sendResetPasswordRequest(this.userId, this.code, values.password)
        .subscribe(() => {
          this.alertify.success("パスワードを変更しました");
          this.router.navigate(['/home']);
        }, (error) => {
          this.alertify.error(error);
        })
      // this.store.dispatch(new AccountActions.TrySignup({registerInfo: this.registerInfo}));
    }

  }
}
