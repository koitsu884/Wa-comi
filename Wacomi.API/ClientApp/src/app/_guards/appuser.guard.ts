
import {map, take} from 'rxjs/operators';
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class AppUserGuard implements CanActivate {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router,
                private alertify: AlertifyService){}

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if(!route.params.appUserId)
        {
            this.alertify.error("ユーザーIDが設定されていません");
            this.router.navigate(['/']);
            return false;
        }
        return this.store.select('account').pipe(
        take(1),
        map((authState: fromAccount.State) => {
            if(authState.appUser.id != route.params.appUserId)
            {
                this.alertify.error("ユーザーIDが一致しません");
                this.router.navigate(['/']);
                return false;
            }
            return true;
        }),)
    }
}