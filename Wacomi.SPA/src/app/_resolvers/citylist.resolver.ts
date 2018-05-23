import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { City } from "../_models/City";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import * as fromGlobal from "../store/global.reducers";
import * as fromApp from "../store/app.reducer";

@Injectable()
export class CityListResolver implements Resolve<City[]> {
    constructor(private store: Store<fromApp.AppState>, 
         private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<City[]> {
        // return this.globalService.getCities()
        // .catch(error => {
        //     console.log('Error occured when getting city list');
        //     this.router.navigate(['/home']);
        //     return Observable.of(null);
        // })
        return this.store.select('global')
        .take(1)
        .switchMap((state : fromGlobal.State) => {
            return Observable.of<City[]>(state.cityList);
        })
        .catch((error) => {
            console.log('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}