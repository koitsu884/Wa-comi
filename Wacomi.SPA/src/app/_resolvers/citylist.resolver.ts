
import {take} from 'rxjs/operators';
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { City } from "../_models/City";
import { Observable } from "rxjs";


import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import * as fromGlobal from "../store/global.reducers";
import * as fromApp from "../store/app.reducer";

@Injectable()
export class CityListResolver implements Resolve<City[]> {
    constructor(private store: Store<fromApp.AppState> ){}

    resolve(route: ActivatedRouteSnapshot) : City[] {
        let cityList = [];
        this.store.select('global').pipe(
        take(1))
        .subscribe((state) => {
            cityList = state.cityList;
        });

        return cityList;
    }
}