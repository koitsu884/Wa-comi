
import {of as observableOf,  Observable } from 'rxjs';

import {catchError, switchMap, take} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { GlobalService } from "../_services/global.service";
import { KeyValue } from "../_models/KeyValue";

import { Store } from "@ngrx/store";
import * as fromApp from "../store/app.reducer";
import * as fromGlobal from "../store/global.reducers";

@Injectable()
export class ClanSeekCategoryResolver implements Resolve<KeyValue[]> {
    constructor(private store: Store<fromApp.AppState>, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<KeyValue[]> {
        return this.store.select('global').pipe(
        take(1),
        switchMap((state : fromGlobal.State) => {
            return observableOf<KeyValue[]>(state.clanSeekCategories);
        }),
        catchError((error) => {
            console.log('Problem retrieving data');
            this.router.navigate(['/']);
            return observableOf(null);
        }),);
    }
}