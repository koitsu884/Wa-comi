import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
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
        this.store.select('global')
        .take(1)
        .subscribe((state) => {
            homeTownList = state.homeTownList;
        });

        return homeTownList;
    }
}