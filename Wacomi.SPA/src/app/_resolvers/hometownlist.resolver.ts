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

    resolve(route: ActivatedRouteSnapshot) : Observable<KeyValue[]> {
        // return this.globalService.getHometowns()
        // .catch(error => {
        //     console.log('Error occured when getting home town list');
        //     this.router.navigate(['/home']);
        //     return Observable.of(null);
        // })
        return this.store.select('global')
        .take(1)
        .switchMap((state : fromGlobal.State) => {
            return Observable.of<KeyValue[]>(state.homeTownList);
        })
        .catch((error) => {
            console.log('Problem retrieving data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}