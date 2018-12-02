import { Resolve, ActivatedRouteSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable()
export class StaticPageResolver implements Resolve<string> {
    constructor(private httpClient: HttpClient) { }

    resolve(route: ActivatedRouteSnapshot): Observable<string> {
        return this.httpClient.get("/assets/static/" + route.paramMap.get('pageName') + ".html", {responseType: 'text'});
    }
}
