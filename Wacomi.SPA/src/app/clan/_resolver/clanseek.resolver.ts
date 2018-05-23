import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { ClanSeek } from "../../_models/ClanSeek";

import * as fromClan from "../store/clan.reducers";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";

@Injectable()
export class ClanSeekResolver implements Resolve<ClanSeek> {
    constructor(private store: Store<fromClan.FeatureState>, 
                private router: Router){}

    resolve(route: ActivatedRouteSnapshot) : Observable<ClanSeek> {
        return this.store.select('clan')
                .take(1)
                .switchMap((state : fromClan.State) => {
                    return Observable.of<ClanSeek>(state.clan);
                })
                .catch((error) => {
                    console.log('Failed to resolve clanseek');
                    this.router.navigate(['/home']);
                    return Observable.of(null);
                });
    }
}