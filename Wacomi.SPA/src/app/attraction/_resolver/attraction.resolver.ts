import * as fromAttraction from '../store/attraction.reducers';
import * as AttractionActions from '../store/attraction.actions';
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Attraction } from "../../_models/Attraction";
import { Store } from "@ngrx/store";
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AttractionResolver implements Resolve<Attraction> {
    constructor(private store: Store<fromAttraction.FeatureState>, 
                private router: Router){}

    resolve(route: ActivatedRouteSnapshot) : Attraction {
        let attraction = null;
        this.store.select('attraction')
        .take(1)
        .subscribe((state) => {
            if(state.selectedAttraction != null)
            {
                attraction = Object.assign({},state.selectedAttraction);
            }
        });

        return attraction;
    }
}