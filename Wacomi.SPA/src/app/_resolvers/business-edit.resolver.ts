import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';
import { AlertifyService } from "../_services/alertify.service";

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';
import { BusinessProfile } from "../_models/BusinessProfile";

@Injectable()
export class BusinessEditResolver implements Resolve<BusinessProfile> {
    constructor(private store: Store<fromApp.AppState>,
        private router: Router,
        private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): BusinessProfile {
        let businessProfile: BusinessProfile = null;
        this.store.select('account')
            .take(1)
            .subscribe((state) => {
                if(state.businessProfile)
                    businessProfile = Object.assign({}, state.businessProfile);
            });
        return businessProfile;
    }
}