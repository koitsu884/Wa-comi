import { Injectable } from "@angular/core";
import { Resolve, Router } from "@angular/router";

import * as fromCircle from "../store/circle.reducers";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs/Observable";
import { CircleEvent } from "../../_models/CircleEvent";

@Injectable()
export class CircleEventResolver implements Resolve<CircleEvent> {
    constructor(private store: Store<fromCircle.FeatureState>, 
                private router: Router){}

    resolve() : Observable<CircleEvent> {
        return this.store.select('circleModule')
                .take(1)
                .switchMap((state) => {
                    return Observable.of<CircleEvent>(state.circleEvent.selectedCircleEvent);
                })
                .catch(() => {
                    console.log('Failed to resolve clanseek');
                    this.router.navigate(['/']);
                    return Observable.of(null);
                });
    }
}