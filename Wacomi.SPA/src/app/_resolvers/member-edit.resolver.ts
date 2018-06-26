import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../_services/alertify.service";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';

import * as fromApp from '../store/app.reducer';
import * as fromAccount from '../account/store/account.reducers';
import * as AccountActions from '../account/store/account.actions';
import { Store } from "@ngrx/store";
import { MemberProfile } from "../_models/MemberProfile";

@Injectable()
export class MemberEditResolver implements Resolve<MemberProfile> {
    constructor(private store: Store<fromApp.AppState>,
        private router: Router,
        private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): MemberProfile {
        let memberProfile: MemberProfile = null;
        this.store.select('account')
            .take(1)
            .subscribe((state) => {
                memberProfile = Object.assign({},state.memberProfile);
            });
        return memberProfile;
    }
}