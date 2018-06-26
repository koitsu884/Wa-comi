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
        private router: Router) { }

    resolve(route: ActivatedRouteSnapshot): Blog[] {
        let blogs = null;
        this.store.select('blogs')
            .take(1)
            .subscribe((state) => {
                blogs = Object.assign([], state.blogs);
            });

        return blogs;
    }
}