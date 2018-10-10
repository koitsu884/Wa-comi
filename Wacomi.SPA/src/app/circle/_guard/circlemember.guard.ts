import * as fromCircle from '../store/circle.reducers';
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';


@Injectable()
export class CircleMemberGuard implements CanActivate {
    constructor(private store: Store<fromCircle.FeatureState>,
                private router: Router){}

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.store.select('circleModule')
        .take(1)
        .map((circleModuleState) => {
            if(circleModuleState.circle.selectedCircle && circleModuleState.circle.selectedCircle.isMember)
               return true;
            this.router.navigate(['/circle']);
            return false;
        })
    }
}