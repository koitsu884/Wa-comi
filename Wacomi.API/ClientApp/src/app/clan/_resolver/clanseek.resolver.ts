
import {of as observableOf,  Observable } from 'rxjs';

import {catchError, switchMap, take} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, Router } from "@angular/router";
import { ClanSeek } from "../../_models/ClanSeek";

import * as fromClan from "../store/clan.reducers";
import { Store } from "@ngrx/store";

@Injectable()
export class ClanSeekResolver implements Resolve<ClanSeek> {
    constructor(private store: Store<fromClan.FeatureState>, 
                private router: Router){}

    resolve(route: ActivatedRouteSnapshot) : Observable<ClanSeek> {
        return this.store.select('clan').pipe(
                take(1),
                switchMap((state : fromClan.State) => {
                    return observableOf<ClanSeek>(state.editingClan);
                }),
                catchError((error) => {
                    console.log('Failed to resolve clanseek');
                    this.router.navigate(['/']);
                    return observableOf(null);
                }),);
    }
}