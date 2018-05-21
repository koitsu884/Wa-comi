import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { GlobalService } from "../_services/global.service";
import { Observable } from "rxjs/Observable";


@Injectable()
export class ClanSeekCategoryResolver implements Resolve<{id:number, name:string}[]> {
    constructor(private globalService: GlobalService, private router: Router ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<{id:number, name:string}[]> {
        return this.globalService.getClanSeekCategories()
        .catch(error => {
            console.log('Error occured when getting clan seek category list');
            this.router.navigate(['/home']);
            return Observable.of(null);
        })
    }
}