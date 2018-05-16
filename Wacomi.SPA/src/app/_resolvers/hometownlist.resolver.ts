import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { GlobalService } from "../_services/global.service";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { Injectable } from "@angular/core";
import { Hometown } from "../_models/Hometown";

@Injectable()
export class HomeTownListResolver implements Resolve<Hometown[]> {
    constructor(private globalService: GlobalService, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<Hometown[]> {
        return this.globalService.getHometowns()
        .catch(error => {
            console.log('Error occured when getting home town list');
            this.router.navigate(['/home']);
            return Observable.of(null);
        })
    }
}