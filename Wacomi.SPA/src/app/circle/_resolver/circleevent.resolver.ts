
import {of as observableOf,  Observable } from 'rxjs';

import {catchError, switchMap, take} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Resolve, Router } from "@angular/router";

import * as fromCircle from "../store/circle.reducers";
import { Store } from "@ngrx/store";
import { CircleEvent } from "../../_models/CircleEvent";

@Injectable()
export class CircleEventResolver implements Resolve<CircleEvent> {
    constructor(private store: Store<fromCircle.FeatureState>, 
                private router: Router){}

    resolve() : Observable<CircleEvent> {
        return this.store.select('circleModule').pipe(
                take(1),
                switchMap((state) => {
                    return observableOf<CircleEvent>(state.circleEvent.selectedCircleEvent);
                }),
                catchError(() => {
                    console.log('Failed to resolve clanseek');
                    this.router.navigate(['/']);
                    return observableOf(null);
                }),);
    }
}