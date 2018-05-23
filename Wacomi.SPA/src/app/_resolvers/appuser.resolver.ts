import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { AppUser } from "../_models/AppUser";

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';
import { Store } from "@ngrx/store";
import { BusinessUser } from "../_models/BusinessUser";
import { Observable } from "rxjs/Observable";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class AppUserResolver implements Resolve<AppUser> {
    constructor(private store: Store<fromApp.AppState>, 
                private alertify: AlertifyService, 
                private router: Router){}

    resolve(route: ActivatedRouteSnapshot) : Observable<AppUser> {
        return this.store.select('account')
                .take(1)
                .switchMap((state : fromAccount.State) => {
                    return Observable.of<AppUser>(state.appUser);
                })
                .catch((error) => {
                    this.alertify.error('Problem retrieving data');
                    this.router.navigate(['/home']);
                    return Observable.of(null);
                });

        // const appUser = JSON.parse(localStorage.getItem('appUser'));
        // return this.businessService.getBusinessUser(appUser.relatedUserClassId).catch(error => {
        //     this.alertify.error('Problem retrieving data');
        //     this.router.navigate(['/home']);
        //     return Observable.of(null);
        // })
    }
}