import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import * as fromPhoto from "../photo/store/photos.reducers";
import * as fromApp from "../store/app.reducer";
import { Photo } from "../_models/Photo";

@Injectable()
export class UserPhotoResolver implements Resolve<Photo[]> {
    constructor(private store: Store<fromApp.AppState>,
        private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Photo[] {
        let photos = null;
        this.store.select('photos')
            .take(1)
            .subscribe((state) => {
                photos = Object.assign([], state.photos);
            });

        return photos;
    }

    // resolve(route: ActivatedRouteSnapshot) : Observable<Photo[]> {

    //     return this.store.select('photos')
    //     .take(1)
    //     .switchMap((state : fromPhoto.State) => {
    //         return Observable.of<Photo[]>(state.photos);
    //     })
    //     .catch((error) => {
    //         console.log('Problem retrieving data');
    //         // this.router.navigate(['/home']);
    //         return Observable.of(null);
    //     });
    // }
}