import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import * as fromBlog from "../blog/store/blogs.reducers";
import * as fromApp from "../store/app.reducer";
import { Blog } from "../_models/Blog";

@Injectable()
export class UserBlogResolver implements Resolve<Blog[]> {
    constructor(private store: Store<fromApp.AppState>, 
         private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<Blog[]> {

        return this.store.select('blogs')
        .take(1)
        .switchMap((state : fromBlog.State) => {
            return Observable.of<Blog[]>(state.blogs);
        })
        .catch((error) => {
            console.log('Problem retrieving data');
            // this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}