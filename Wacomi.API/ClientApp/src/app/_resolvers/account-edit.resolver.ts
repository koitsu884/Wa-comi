
import {take} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { UserAccount } from "../_models/UserAccount";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs";
import { Store } from "@ngrx/store";
import { AlertifyService } from "../_services/alertify.service";
import * as fromApp from "../store/app.reducer";
import * as fromAccount from "../account/store/account.reducers";

@Injectable()
export class AccountEditResolver implements Resolve<UserAccount> {
    constructor(private store: Store<fromApp.AppState>,
        private router: Router,
        private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): UserAccount {
        let userAccount = null;
        this.store.select('account').pipe(
            take(1))
            .subscribe((state) => {
                if(state.account)
                    userAccount = Object.assign({}, state.account);
            });

        return userAccount;
    }

    // resolve(route: ActivatedRouteSnapshot): Observable<UserAccount> {
    //     return this.store.select('account')
    //         .take(1)
    //         .switchMap((state: fromAccount.State) => {
    //             return Observable.of(state.account);
    //         })
    //         .catch((error) => {
    //             this.alertify.error('Problem retrieving data');
    //             this.router.navigate(['/']);
    //             return Observable.of(null);
    //         });
    // }
}