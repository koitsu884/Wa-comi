import { FormGroup } from "@angular/forms";

export class PasswordEditorBase{
    passwordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,20}";
    passwordMinLength = 6;
    passwordMaxLength = 20;

    constructor(){};

    passwordMatchValidator(g: FormGroup){
        return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch' : true};
    }
}