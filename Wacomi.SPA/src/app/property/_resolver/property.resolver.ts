
import {take} from 'rxjs/operators';
import * as fromProperty from '../store/property.reducers';
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Store } from "@ngrx/store";
import { Observable } from 'rxjs';
import { Property } from '../../_models/Property';

@Injectable()
export class PropertyResolver implements Resolve<Property> {
    constructor(private store: Store<fromProperty.FeatureState>){}

    resolve(route: ActivatedRouteSnapshot) : Property {
        let property = null;
        this.store.select('property').pipe(
        take(1))
        .subscribe((state) => {
            if(state.selectedProperty != null)
            {
                property = Object.assign({},state.selectedProperty);
            }
        });

        return property;
    }
}