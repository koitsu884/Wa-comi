import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';
import { AlertifyService } from "../_services/alertify.service";

import * as fromApp from '../store/app.reducer';
import * as fromClan from '../clan/store/clan.reducers';
import * as ClanActions from '../clan/store/clan.actions';
import { ClanSeek } from "../_models/ClanSeek";

@Injectable()
export class ClanSeekEditResolver implements Resolve<ClanSeek> {
    constructor(private store: Store<fromClan.FeatureState>,
                private router: Router, 
                private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot) : Observable<ClanSeek> {
        return this.store.select('clan')
        .take(1)
        .switchMap((state : fromClan.State) => {
            return Observable.of(state.editingClan);
        })
        .catch((error) => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}