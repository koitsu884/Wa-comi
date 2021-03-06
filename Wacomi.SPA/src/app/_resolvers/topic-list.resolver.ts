
import {of as observableOf,  Observable } from 'rxjs';

import {catchError} from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { AlertifyService } from "../_services/alertify.service";
import { DailyTopic } from "../_models/DailyTopic";
@Injectable()
export class TopicListResolver implements Resolve<DailyTopic[]> {
    baseUrl = environment.apiUrl;
    
    constructor(private router: Router, private alertify: AlertifyService, private httpClient: HttpClient ){}

    resolve(route: ActivatedRouteSnapshot) : Observable<DailyTopic[]> {
        let userId = route.params.userId;
        return this.httpClient.get<DailyTopic[]>(this.baseUrl + 'dailytopic?userId=' + userId).pipe(
        catchError(error => {
            this.alertify.error('Problem getting member data');
            this.router.navigate(['/']);
            return observableOf(null);
        }))
    }
}