
import {map, take} from 'rxjs/operators';
import * as fromCircle from '../store/circle.reducers';
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Injectable } from "@angular/core";
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';


@Injectable()
export class CircleOwnerGuard implements CanActivate {
    constructor(private store: Store<fromCircle.FeatureState>,
                private router: Router){}

    canActivate(route: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.store.select('circleModule').pipe(
        take(1),
        map((circleModuleState) => {
            if(circleModuleState.circle.selectedCircle && circleModuleState.circle.selectedCircle.isManageable)
               return true;
            this.router.navigate(['/circle']);
            return false;
        }),)
    }
}