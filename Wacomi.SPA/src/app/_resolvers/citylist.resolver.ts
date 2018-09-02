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
    constructor(private store: Store<fromApp.AppState> ){}

    resolve(route: ActivatedRouteSnapshot) : City[] {
        let cityList = [];
        this.store.select('global')
        .take(1)
        .subscribe((state) => {
            cityList = state.cityList;
        });

        return cityList;
    }
}