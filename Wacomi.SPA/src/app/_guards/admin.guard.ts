import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { Injectable } from "@angular/core";
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class AdminGuard implements CanActivate {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router,
                private alertify: AlertifyService){}

    canActivate(): Observable<boolean> | Promise<boolean> | boolean {
        return this.store.select('account')
        .take(1)
        .map((authState: fromAccount.State) => {
            if(authState.authenticated && authState.appUser.userType == "Admin"){
                return true;
            }
            else{
                this.alertify.error("このページにアクセスするには管理アカウントでログインしてください");
                this.router.navigate(['/']);
                return false;
            }
        })
    }
}