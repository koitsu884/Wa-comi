import * as fromApp from '../store/app.reducer';
import { Injectable } from "@angular/core";
import { Resolve, Router } from "@angular/router";
import { Store } from "@ngrx/store";
import { Category } from '../_models/Category';

@Injectable()
export class AttractionCategoryResolver implements Resolve<Category[]> {
    constructor(private store: Store<fromApp.AppState>, private router: Router ){}

    resolve() : Category[] {
        let categories: Category[];
        this.store.select('global')
        .take(1)
        .subscribe((state) => {
            categories = state.attractionCategories;
        });

        return categories;
    }
}