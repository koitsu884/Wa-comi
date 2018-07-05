import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { AppUser } from "../_models/AppUser";

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import { AlertifyService } from "../_services/alertify.service";

@Injectable()
export class AppUserResolver implements Resolve<AppUser> {
    constructor(private store: Store<fromApp.AppState>, 
                private alertify: AlertifyService, 
                private router: Router){}

    resolve(route: ActivatedRouteSnapshot) : AppUser {
        let appUser = null;
        this.store.select('account')
        .take(1)
        .subscribe((state) => {
            if(state.appUser)
            {
                appUser = Object.assign({},state.appUser);
            }
        });

        return appUser;
    }
}