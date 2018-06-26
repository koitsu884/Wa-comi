import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { GlobalService } from "../_services/global.service";
import { Observable } from "rxjs/Observable";
import { KeyValue } from "../_models/KeyValue";

import { Store } from "@ngrx/store";
import * as fromApp from "../store/app.reducer";
import * as fromGlobal from "../store/global.reducers";

@Injectable()
export class ClanSeekCategoryResolver implements Resolve<KeyValue[]> {
    constructor(private store: Store<fromApp.AppState>, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<KeyValue[]> {
        return this.store.select('global')
        .take(1)
        .switchMap((state : fromGlobal.State) => {
            return Observable.of<KeyValue[]>(state.clanSeekCategories);
        })
        .catch((error) => {
            console.log('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}