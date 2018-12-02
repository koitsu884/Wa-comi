
import {take} from 'rxjs/operators';
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs";


import { Injectable } from "@angular/core";
import { Hometown } from "../_models/Hometown";
import { Store } from "@ngrx/store";
import * as fromGlobal from "../store/global.reducers";
import * as fromApp from "../store/app.reducer";
import { KeyValue } from "../_models/KeyValue";

@Injectable()
export class HomeTownListResolver implements Resolve<KeyValue[]> {
    constructor(private store: Store<fromApp.AppState>, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : KeyValue[] {
        let homeTownList = [];
        this.store.select('global').pipe(
        take(1))
        .subscribe((state) => {
            homeTownList = state.homeTownList;
        });

        return homeTownList;
    }
}