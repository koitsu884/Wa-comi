import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { City } from "../_models/City";
import { GlobalService } from "../_services/global.service";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Injectable } from "@angular/core";

@Injectable()
export class CityListResolver implements Resolve<City[]> {
    constructor(private globalService: GlobalService, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<City[]> {
        return this.globalService.getCities()
        .catch(error => {
            console.log('Error occured when getting city list');
            this.router.navigate(['/home']);
            return Observable.of(null);
        })
    }
}