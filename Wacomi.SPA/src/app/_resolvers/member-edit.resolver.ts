import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Member } from "../_models/Member";
import { AlertifyService } from "../_services/alertify.service";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';
import { Store } from "@ngrx/store";

@Injectable()
export class MemberEditResolver implements Resolve<Member> {
    constructor(private store: Store<fromApp.AppState>,
                private router: Router, 
                private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot) : Observable<Member> {
        return this.store.select('account')
        .take(1)
        .switchMap((state : fromAccount.State) => {
            return Observable.of(state.memberProfile);
        })
        .catch((error) => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}