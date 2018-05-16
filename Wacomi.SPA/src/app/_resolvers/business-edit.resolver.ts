import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';
import { BusinessUser } from "../_models/BusinessUser";
import { AlertifyService } from "../_services/alertify.service";

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';

@Injectable()
export class BusinessEditResolver implements Resolve<BusinessUser> {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router, 
                private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot) : Observable<BusinessUser> {
        return this.store.select('account')
        .take(1)
        .switchMap((state : fromAccount.State) => {
            return Observable.of(state.bisuserProfile);
        })
        .catch((error) => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}