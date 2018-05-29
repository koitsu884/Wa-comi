import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { Member } from "../_models/Member";
import { Observable } from "rxjs/Observable";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { AlertifyService } from "../_services/alertify.service";
@Injectable()
export class MemberDetailResolver implements Resolve<Member> {
    baseUrl = environment.apiUrl;
    
    constructor(private router: Router, private alertify: AlertifyService, private httpClient: HttpClient ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<Member> {
        let id = route.params.id;
        return this.httpClient.get<Member>(this.baseUrl + 'member/' + id)
        .catch(error => {
            this.alertify.error('Problem getting member data');
            this.router.navigate(['/home']);
            return Observable.of(null);
        })
    }
}