import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { AlertifyService } from "../_services/alertify.service";
import { UserDetails } from "../_models/UserDetails";
@Injectable()
export class UserDetailResolver implements Resolve<UserDetails> {
    baseUrl = environment.apiUrl;
    
    constructor(private router: Router, private alertify: AlertifyService, private httpClient: HttpClient ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<UserDetails> {
        return this.httpClient.get<UserDetails>(this.baseUrl + 'appuser/' + route.params.id + '/detail') 
        .catch(error => {
            this.alertify.error('Problem getting member data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        })
    }
}