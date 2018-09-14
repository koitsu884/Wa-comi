import * as fromApp from '../store/app.reducer';
import { Injectable } from "@angular/core";
import { Resolve} from "@angular/router";
import { Store } from "@ngrx/store";
import { Category } from '../_models/Category';

@Injectable()
export class PropertyCategoryResolver implements Resolve<Category[]> {
    constructor(private store: Store<fromApp.AppState>){}

    resolve() : Category[] {
        let categories: Category[];
        this.store.select('global')
        .take(1)
        .subscribe((state) => {
            categories = state.propertyCategories;
        });

        return categories;
    }
}