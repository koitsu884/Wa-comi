
import {map, take} from 'rxjs/operators';
import { CanActivate, Router } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class MemberGuard implements CanActivate {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router,
                private alertify: AlertifyService){}

    canActivate(): Observable<boolean> | Promise<boolean> | boolean {
        return this.store.select('account').pipe(
        take(1),
        map((authState: fromAccount.State) => {
            if(authState.authenticated && authState.appUser.userType == "Member"){
                return true;
            }
            else{
                this.alertify.error("このページにアクセスするには個人アカウントでログインしてください");
                this.router.navigate(['/']);
                return false;
            }
        }),)
    }
}