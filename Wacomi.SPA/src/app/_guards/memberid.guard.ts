import { CanActivate, Router, Params, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { Injectable } from "@angular/core";
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class MemberIdGuard implements CanActivate {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router,
                private alertify: AlertifyService){}

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        const memberId = route.params["memberId"];

        return this.store.select('account')
        .take(1)
        .map((authState: fromAccount.State) => {
            if(authState.authenticated && authState.appUser.userType == "Member" && authState.appUser.relatedUserClassId == memberId){
                return true;
            }
            else{
                this.alertify.error("このページにアクセスするには個人アカウントでログインしてください");
                this.router.navigate(['/home']);
                return false;
            }
        })
    }
}